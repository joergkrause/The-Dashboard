using Azure.Identity;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using TheDashboard.Clients;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web;
using Blazorise.RichTextEdit;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;
using TheDashboard.FrontendUi.Services.Mapper;
using TheDashboard.FrontendUi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Rewrite;

namespace TheDashboard.FrontendUi
{

  public class Program
  {
    public static async Task Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      builder.Logging.AddAzureWebAppDiagnostics(configure =>
      {
        configure.IncludeScopes = true;
      });

      // all traffic going in the system is routed through the proxy
      builder.Services.AddHttpClient("HttpCommandProxy", client =>
      {
        client.DefaultRequestHeaders.Add("X-Command", "true");
        client.BaseAddress = new Uri("http://proxy:5000");
      });
      builder.Services.AddHttpClient("HttpQueryProxy", client =>
      {
        client.DefaultRequestHeaders.Add("X-Query", "true");
        client.BaseAddress = new Uri("http://proxy:5000");
      });

      builder.Services.AddAzureAppConfiguration();
      builder.Services.AddFeatureManagement().AddFeatureFilter<TimeWindowFilter>();

      var appConfConnectionString = builder.Configuration.GetConnectionString("AppConfig");
      // this is an options, to run outside of Azure use local appsettings.json
      if (!String.IsNullOrEmpty(appConfConnectionString))
      {
        builder.Configuration.AddAzureAppConfiguration(options =>
        {

          options.Connect(appConfConnectionString)
            .UseFeatureFlags(options => options.Label = "Block")
            .ConfigureRefresh(refresh =>
            {
              refresh.Register("FeatureManagement", refreshAll: true).SetCacheExpiration(new TimeSpan(0, 5, 0));
            })
            .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions())
            ))
            .Select(KeyFilter.Any, LabelFilter.Null);
        });
      }

      /* Blazor */
      builder.Services.AddResponseCaching();
      //builder.Services.AddControllers();
      builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
      builder.Services.AddRazorPages();
      builder.Services.AddServerSideBlazor()
        .AddHubOptions(options =>
        {
          options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
          options.EnableDetailedErrors = false;
          options.HandshakeTimeout = TimeSpan.FromSeconds(15);
          options.KeepAliveInterval = TimeSpan.FromSeconds(15);
          options.MaximumParallelInvocationsPerClient = 1;
          options.MaximumReceiveMessageSize = 32 * 1024;
          options.StreamBufferCapacity = 10;
        })
        .AddMicrosoftIdentityConsentHandler();
      builder.Services.AddBlazorise(options =>
      {
        // https://blazorise.com/docs/components/memo
        options.Immediate = false;
        options.Debounce = true;
        options.DebounceInterval = 300;
      })
      .AddBootstrap5Providers()
      .AddFontAwesomeIcons();
      builder.Services.AddBlazoriseRichTextEdit(options => { });

      #region Auth

      builder.Services
        .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)        
        .AddMicrosoftIdentityWebApp(options =>
      {
        builder.Configuration.Bind("AzureAdB2C", options);
      })        
        ;

      builder.Services.AddAuthorization();    

      #endregion Auth

      // Hub
      builder.Services.AddSingleton<ITileDataService, TileDataService>(sp =>
      {
        var logger = sp.GetRequiredService<ILogger<TileDataService>>();
        var ts = new TileDataService(logger, builder.Configuration);
        return ts;
      });

      // Services that address the microservices, using httpclient to route through the proxy
      builder.Services.AddSingleton<IDashboardClient>(sp =>
      {
        var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("HttpQueryProxy");
        // TODO: Add auth
        return new DashboardClient(builder.Configuration["QueryServices:Dashboard"], httpClient);
      });
      builder.Services.AddSingleton<ITilesClient>(sp =>
      {
        var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("HttpQueryProxy");
        return new TilesClient(builder.Configuration["QueryServices:Tiles"], httpClient);
      });
      builder.Services.AddSingleton<IDataConsumerClient>(sp =>
      {
        var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("HttpQueryProxy");
        return new DataConsumerClient(builder.Configuration["QueryServices:DataConsumer"], httpClient);
      });

      builder.Services.AddSingleton<IDashboardService, DashboardService>();

      builder.Services.AddAutoMapper(typeof(ModelMappings).Assembly);

      builder.Services.AddResponseCompression(opts =>
      {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
              new[] { "application/octet-stream" });
      });

      var app = builder.Build();

      if (!app.Environment.IsDevelopment())
      {        
        app.UseExceptionHandler("/Error");
        app.UseHsts();
      }

      // Configure the HTTP request pipeline.
      if (!app.Environment.IsDevelopment())
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();

      app.UseCookiePolicy(new CookiePolicyOptions
      {
        Secure = CookieSecurePolicy.Always,
        MinimumSameSitePolicy = SameSiteMode.None
      });

      app.UseStaticFiles();
      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseRewriter(new RewriteOptions().Add(context =>
    {
      if (context.HttpContext.Request.Path == "/MicrosoftIdentity/Account/SignedOut")
      {
        context.HttpContext.Response.Redirect("/SignedOut");
      }
    }));

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapRazorPages();
        endpoints.MapControllers();
        endpoints.MapBlazorHub().AllowAnonymous().RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = $"{OpenIdConnectDefaults.AuthenticationScheme},{IdentityConstants.ApplicationScheme}" });                
        endpoints.MapFallbackToPage("/_Host");
      });

      // await InitHubAsync(app.Services.GetRequiredService<IServiceProvider>());

      app.Run();

    }

    static async Task InitHubAsync(IServiceProvider serviceProvider)
    {
      var tileDataService = serviceProvider.GetRequiredService<ITileDataService>();
      await tileDataService.Init();
    }


  }
}