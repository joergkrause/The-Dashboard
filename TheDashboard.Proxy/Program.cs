using EventStore.Client;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using TheDashboard.BuildingBlocks.Extensions;
using TheDashboard.Proxy.Middleware;
using TheDashboard.Proxy.Services;
using TheDashboard.SharedEntities;
using Yarp.ReverseProxy.Forwarder;

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

    // builder.Services.AddSingleton<ICommandServiceRepository, CommandServiceRepository>();

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
    app.UseEndpoints(async (endpoints) =>
    {

      //var transformer = new ServiceTransformer(app.Services.GetRequiredService<ICommandServiceRepository>()); // or HttpTransformer.Default;
      // var store = app.Services.GetRequiredService<ICommandServiceRepository>();
      var forwarder = app.Services.GetRequiredService<IHttpForwarder>();
      var eventStoreClient = app.Services.GetRequiredService<EventStoreClient>();
      var busEndpoint = app.Services.GetRequiredService<IPublishEndpoint>();
      var options = new JsonSerializerOptions();
      options.Converters.Add(new CommandConverter());

      async Task Subscribe()
      {
         await eventStoreClient.SubscribeToStreamAsync("some-stream", FromStream.End,
          async (streamName, eventAppeared, dropWithError) =>
          {
            Console.WriteLine($"Event {eventAppeared.Event.EventType} Appeared");
            if (eventAppeared.Event.EventType == "Command")
            {
              var data = eventAppeared.Event.Data;
              var json = Encoding.UTF8.GetString(data.ToArray());
              try
              {
                var typeName = JsonSerializer.Deserialize<string>(eventAppeared.Event.Metadata.ToArray());
                var type = Type.GetType($"TheDashboard.SharedEntities.{typeName}, TheDashboard.SharedEntities") ?? throw new TypeAccessException($"Type {typeName} not found");
                var evt = JsonSerializer.Deserialize(json, type);
                await busEndpoint.Publish(evt, dropWithError);
              }
              catch (Exception)
              {
              }
            }
          }, false, default);
      }

      endpoints.MapReverseProxy(proxyPipeline =>
          {
            proxyPipeline.Use(async (context, next) =>
                {
                  var lf = proxyPipeline.ApplicationServices.GetRequiredService<ILoggerFactory>();
                  var logger = lf.CreateLogger("ReverseProxy");
                  logger.LogInformation("Proxying request: {0}", context.Request.GetDisplayUrl());

                  // filter POST only and retrieve request body
                  if (context.Request.Method == "POST" && context.Request.Headers.ContainsKey("X-Command"))
                  {

                    await Subscribe();

                    context.Request.EnableBuffering();
                    var jsonObject = await JsonDocument.ParseAsync(context.Request.Body);
                    try
                    {
                      var typeName = jsonObject.RootElement.GetProperty("Type").GetString()!;
                      var type = Type.GetType($"TheDashboard.SharedEntities.{typeName}, TheDashboard.SharedEntities") ?? throw new TypeAccessException($"Type {typeName} not found");
                      var evt = jsonObject.Deserialize(type, options);
                      if (evt != null)
                      {
                        var eventData = new EventData(
                            Uuid.NewUuid(),
                            "Command",
                            JsonSerializer.SerializeToUtf8Bytes(evt),
                            JsonSerializer.SerializeToUtf8Bytes(typeName)
                        );
                        // store in event store
                        var result = await eventStoreClient.AppendToStreamAsync("some-stream", StreamState.Any, new[] { eventData });
                        // we have a proper handling achieved and answer this
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        await context.Response.WriteAsync("Command processed");
                        return;
                      }
                      else
                      {
                        logger.LogWarning("Invalid request: {0}", typeName);
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Invalid request");
                        return;
                      }
                    }
                    catch(Exception ex)
                    {
                      logger.LogWarning("Invalid request error {0}", ex.Message);
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