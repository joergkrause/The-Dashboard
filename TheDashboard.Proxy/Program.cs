using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using TheDashboard.BuildingBlocks.Extensions;
using TheDashboard.Proxy.Middleware;
using TheDashboard.Proxy.Services;
using TheDashboard.Proxy.Transformers;
using TheDashboard.SharedEntities;
using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Transforms;

namespace TheDashboard.Proxy;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddHttpForwarder();

        // https://www.kallemarjokorpi.fi/blog/request-routing-in-bff.html
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
            //.AddTransforms(builderContext =>
            //{
            //    var transformer = new ServiceTransformer(builderContext.Services.GetRequiredService<ICommandServiceRepository>());
            //    builderContext.AddRequestTransform(async transformContext =>
            //    {
            //        // TODO: needs auth
            //        var accessToken = await transformContext.HttpContext.GetTokenAsync("access_token");
            //        if (accessToken != null)
            //        {
            //            transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //        }
            //    });
            //});

        /** Event Sourcing using EventStore **/
        builder.Services.AddEventStoreClient(builder.Configuration.GetSection("EventStore")!.Get<string>()!);
        /** Masstransit publishing only, no outbox pattern **/
        builder.Services.AddEventbus(builder.Configuration);

        builder.Services.AddSingleton<ICommandServiceRepository, CommandServiceRepository>();

        var app = builder.Build();

        var env = app.Services.GetRequiredService<IWebHostEnvironment>();
                
        if (env.IsDevelopment())
        {
            app.UseHttpLogging();
            app.UseDeveloperExceptionPage();
        }

        var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
        {
            PreAuthenticate = false,            
            UseProxy = false,
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.None,
            UseCookies = false,
            ActivityHeadersPropagator = new ReverseProxyPropagator(DistributedContextPropagator.Current),
            ConnectTimeout = TimeSpan.FromSeconds(15),
        });

        var requestConfig = new ForwarderRequestConfig { ActivityTimeout = TimeSpan.FromSeconds(100) };

        app.UseRouting();
        app.UseMiddleware<HttpInspectorMiddleware>();
        app.UseEndpoints(endpoints =>
        {

            //var transformer = new ServiceTransformer(app.Services.GetRequiredService<ICommandServiceRepository>()); // or HttpTransformer.Default;
            var store = app.Services.GetRequiredService<ICommandServiceRepository>();
            var forwarder = app.Services.GetRequiredService<IHttpForwarder>();
            var options = new JsonSerializerOptions();
            options.Converters.Add(new CommandConverter());

            endpoints.MapReverseProxy(proxyPipeline =>
            {
                proxyPipeline.Use(async (context, next) =>
                {
                    var lf = proxyPipeline.ApplicationServices.GetRequiredService<ILoggerFactory>();
                    var logger = lf.CreateLogger("ReverseProxy");
                    logger.LogInformation("Proxying request: {0}", context.Request.GetDisplayUrl());

                    // filter POST only and retrieve request body
                    if (context.Request.Method == "POST")
                    {
                        context.Request.EnableBuffering();
                        var requestBody = context.Request.Body;
                        using var reader = new StreamReader(requestBody);
                        var body = await reader.ReadToEndAsync();
                        context.Request.Body.Position = 0;
                        try
                        {
                            var evt = JsonSerializer.Deserialize<Command>(body, options);
                            if (evt != null)
                            {
                                await store.StoreAndPublish(evt, default);
                                // we have a proper handling achieved and answer this
                                context.Response.StatusCode = StatusCodes.Status200OK;
                                await context.Response.WriteAsync("Command processed");
                                return;
                            } else
                            {
                                logger.LogWarning("Invalid request: {0}", body);
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                await context.Response.WriteAsync("Invalid request");
                                return;
                            }
                        }
                        catch
                        {
                            logger.LogWarning("Invalid request error: {0}", body);
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync("Error deserializing request");
                            return;
                        }
                    }

                    await next();
                });
            });

        });

        app.Run();
    }
}