using MassTransit;
using TheDashboard.BuildingBlocks.Extensions;
using TheDashboard.SharedEntities;
using TheDashboard.UiInfoService.Hubs;
using TheDashboard.UiInfoService.Infrastructure.Integration;
using TheDashboard.UiInfoService.Infrastructure.Integration.Models;

namespace TheDashboard.UiInfoService
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      builder.Services.AddDefaultServices();
      // builder.Services.AddLogging(config => config.AddConsole());

      builder.Services.AddMassTransit(x =>
      {
        // listen for messages from dataconsumer service (and others that have something to tell)
        x.AddConsumer<ConsumerHandler>();
        x.UsingRabbitMq((context, cfg) =>
        {
          cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
          {
            h.Username(builder.Configuration["RabbitMq:User"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
          });
          cfg.ReceiveEndpoint("DataConsumerService", e =>
          {
            e.ConfigureConsumers(context);
            e.Bind(typeof(DataEvent).Name);
          });
        });
      });

      builder.Services.AddSignalR();

      // we receive all data through the queue and push them to the clients through SignalR
      builder.Services.AddTransient<ConsumerHandler>();

      builder.Services.AddAuthorization();

      // CORS probably not required if data pulling comes from server side blazor code
      builder.Services.AddCors(options =>
      {
        options.AddPolicy("AllowBlazorApp",
            builder =>
            { 
              builder.WithOrigins("http://localhost", "http://localhost:5500", "http://frontend", "http://frontend:5500", "https://localhost", "https://localhost:7500", "https://frontend", "https://frontend:7500")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials(); // SignalR requires this
            });
      });

      var app = builder.Build();

      app.UseDefaultConfiguration("AllowBlazorApp");

      app.MapControllers();
      app.MapHub<InfoHub>("/TileData").RequireCors("AllowBlazorApp");

      app.Run();
    }
  }
}