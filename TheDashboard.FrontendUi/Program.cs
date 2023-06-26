using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;
using TheDashboard.Clients;
using TheDashboard.FrontendUi.Hubs;

namespace TheDashboard.FrontendUi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      builder.Services.AddRazorPages();
      builder.Services.AddServerSideBlazor();

      builder.Services.AddSingleton<IDashboardClient, DashboardClient>();
      builder.Services.AddSingleton<ITilesClient, TilesClient>();
      builder.Services.AddSingleton<IDataConsumerClient, DataConsumerClient > ();

      builder.Services.AddResponseCompression(opts =>
      {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
              new[] { "application/octet-stream" });
      });

      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (!app.Environment.IsDevelopment())
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      
      app.UseHttpsRedirection();
      app.UseResponseCompression();

      app.UseStaticFiles();

      app.UseRouting();

      app.MapBlazorHub();
      app.MapFallbackToPage("/_Host");

      app.MapHub<DataHub>("/datahub");

      app.Run();
    }
  }
}