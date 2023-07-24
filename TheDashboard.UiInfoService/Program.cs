using TheDashboard.BuildingBlocks.Extensions;
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

      builder.Services.AddSignalR();

      // we receive all data through the queue and push them to the clients
      builder.Services.AddTransient<ConsumerHandler<TileData>>();

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
            {
              builder.WithOrigins("https://localhost")
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
      
      app.UseDefaultConfiguration();
      app.UseCors("AllowBlazorApp");

      app.MapHub<InfoHub>("/TileData").RequireCors("AllowBlazorApp");

      app.Run();
    }
  }
}