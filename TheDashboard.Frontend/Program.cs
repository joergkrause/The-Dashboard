using Frontend.Proxy;
using Frontend.Services;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection.Metadata.Ecma335;
using static GrpcBackend.Dashboard;
using static System.Net.WebRequestMethods;

namespace Frontend
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddRazorPages();
      builder.Services.AddServerSideBlazor();

      // builder.Services.AddSingleton<DashboardProxy>(new DashboardProxy("https://localhost:7165", new HttpClient()));
      builder.Services.AddScoped<BackendService>(); // REST

      builder.Services.AddSingleton<DashboardClient>(sp =>
      {
        var channel = GrpcChannel.ForAddress("https://localhost:7116");
        return new DashboardClient(channel);
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

      app.UseStaticFiles();

      app.UseRouting();

      app.MapBlazorHub();
      app.MapFallbackToPage("/_Host");

      app.Run();
    }
  }
}