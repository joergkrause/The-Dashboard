using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using TheDashboard.BuildingBlocks.Extensions;
using TheDashboard.Proxy.Middleware;
using TheDashboard.Proxy.Services;
using TheDashboard.Proxy.Transformers;
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

            endpoints.MapReverseProxy(proxyPipeline =>
            {
                proxyPipeline.Use((context, next) =>
                {
                    var lf = proxyPipeline.ApplicationServices.GetRequiredService<ILoggerFactory>();
                    var logger = lf.CreateLogger("ReverseProxy");
                    logger.LogInformation("Proxying request: {0}", context.Request.GetDisplayUrl());
                    return next();
                });
            });

            // TODO: Get POST working and forward to store service

            endpoints.MapGet("api/query/{route}/{**catch-all}", async httpContext =>
            {
                // get part of route and store in variable
                var route = httpContext.Request.RouteValues["route"];
                var remainder = httpContext.Request.RouteValues["catch-all"];

                // await store.StoreAndPublish(, CancellationToken.None);

                var requestUrl = $"http://TheDashboard.{route}/{remainder}";
                var error = await forwarder.SendAsync(httpContext, requestUrl,
                httpClient, requestConfig, HttpTransformer.Default);
                // Check if the operation was successful
                if (error != ForwarderError.None)
                {
                    var errorFeature = httpContext.GetForwarderErrorFeature();
                    var exception = errorFeature.Exception;
                }
            });
        });

        app.Run();
    }
}