using MassTransit;
using TheDashboard.BuildingBlocks.Extensions;
using TheDashboard.DatabaseLayer.Domain.Contracts;
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
      builder.Services.AddLogging(config => config.AddConsole());

      builder.Services.AddMassTransit(x =>
      {
        // listen for messages from dataconsumer service (and others that have something to tell)
        x.AddConsumer<ConsumerHandler<DataConsumerMessage>>();
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
            // TODO: use typesafe form
            e.Bind("TheDashboard.DatabaseLayer.Domain.Contracts:DataConsumerMessage");
          });
        });
      });
      
      builder.Services.AddSignalR();

      // we receive all data through the queue and push them to the clients
      builder.Services.AddTransient<ConsumerHandler<DataConsumerMessage>>();

      builder.Services.AddSwaggerGen(config =>
      {
        config.SwaggerDoc("v1", new() { Title = "UiInfo API", Version = "v1" });
      });      

      builder.Services.AddAuthorization();

      // CORS probably not required if data pulling comes from server side blazor code
      builder.Services.AddCors(options =>
      {
        options.AddPolicy("AllowBlazorApp",
            builder =>
            { // .WithOrigins("https://localhost")
              builder.WithOrigins("https://localhost", "http://localhost:5500", "http://frontend", "http://frontend:5500")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials(); // SignalR requires this
            });
      });

      var app = builder.Build();      

      app.UseSwagger();
      app.UseSwaggerUI(config =>
      {
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "UiInfo API v1");
      });
      
      app.UseDefaultConfiguration("AllowBlazorApp");

      app.MapControllers();
      app.MapHub<InfoHub>("/TileData").RequireCors("AllowBlazorApp");

      app.Run();
    }
  }
}