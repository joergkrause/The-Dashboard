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

      // Add services to the container.

      builder.Services.AddSignalR();
      builder.Services.AddTransient<ConsumerHandler<TileData>>();

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

      // Configure the HTTP request pipeline.

      app.UseHttpsRedirection();
      app.UseCors("AllowBlazorApp");
      app.UseAuthorization();

      app.MapHub<InfoHub>("/Info").RequireCors("AllowBlazorApp");

      app.Run();
    }
  }
}