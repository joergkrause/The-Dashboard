using System.Diagnostics;
using System.Net;
using TheDashboard.BuildingBlocks.Extensions;
using TheDashboard.Proxy.Services;
using TheDashboard.Proxy.Transformers;
using Yarp.ReverseProxy.Forwarder;

namespace TheDashboard.Proxy;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpForwarder();
        builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

        var app = builder.Build();

        var env = app.Services.GetRequiredService<IWebHostEnvironment>();
        var forwarder = app.Services.GetRequiredService<IHttpForwarder>();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        /** Event Sourcing using EventStore **/
        builder.Services.AddEventStoreClient(builder.Configuration.GetSection("EventStore")!.Get<string>()!);
        /** Masstransit publishing only, no outbox pattern **/
        builder.Services.AddEventbus(builder.Configuration);

        builder.Services.AddSingleton<ICommandServiceRepository, CommandServiceRepository>();

        var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
        {
            UseProxy = false,
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.None,
            UseCookies = false,
            ActivityHeadersPropagator = new ReverseProxyPropagator(DistributedContextPropagator.Current),
            ConnectTimeout = TimeSpan.FromSeconds(15),
        });

        var requestConfig = new ForwarderRequestConfig { ActivityTimeout = TimeSpan.FromSeconds(100) };

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {

            var transformer = new ServiceTransformer(app.Services.GetRequiredService<ICommandServiceRepository>()); // or HttpTransformer.Default;

            // endpoints.MapReverseProxy();
            endpoints.Map("/{**catch-all}", async httpContext =>
          {
              // get part of route and store in variable
              var route = httpContext.Request.Path.Value;

              var error = await forwarder.SendAsync(httpContext, $"http://{route}/",
              httpClient, requestConfig, transformer);
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