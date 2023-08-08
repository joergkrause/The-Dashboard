using Azure.Identity;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazorise.RichTextEdit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Prometheus;
using TheDashboard.Frontend.Services;
using TheDashboard.Frontend.Services.Mapper;
using TheDashboard.SharedEntities;

namespace TheDashboard.Frontend;


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

    //// This is only for the cloud-native variant
    //// this is an options, to run outside of Azure use local appsettings.json
    //builder.Services.AddAzureAppConfiguration();
    //builder.Services.AddFeatureManagement().AddFeatureFilter<TimeWindowFilter>();1
    //var appConfConnectionString = builder.Configuration.GetConnectionString("AppConfig");
    //if (!String.IsNullOrEmpty(appConfConnectionString))
    //{
    //  builder.Configuration.AddAzureAppConfiguration(options =>
    //  {

    //    options.Connect(appConfConnectionString)
    //      .UseFeatureFlags(options => options.Label = "Block")
    //      .ConfigureRefresh(refresh =>
    //      {
    //        refresh.Register("FeatureManagement", refreshAll: true).SetCacheExpiration(new TimeSpan(0, 5, 0));
    //      })
    //      .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions())
    //      ))
    //      .Select(KeyFilter.Any, LabelFilter.Null);
    //  });
    //}

    /* Blazor */
    builder.Services.AddResponseCaching();
    //builder.Services.AddControllers();
    builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
    builder.Services.AddRazorPages();
    builder.Services.AddWebOptimizer(options =>
    {
      options.AddCssBundle("/bundle.css",
        "/vendor/bootstrap/css/bootstrap.css",
        "/vendor/boxicons/css/boxicons.css",
        "/vendor/boxicons/css/animations.css",
        "/vendor/boxicons/css/transformations.css",
        "/css/site.css"
      ).MinifyCss();
      options.AddJavaScriptBundle("/bundle.js",
        "/vendor/bootstrap/js/bootstrap.bundle.js",
        "/dashboard.js"
      ).MinifyJavaScript();
    });


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
    // builder.Services.AddBlazoriseRichTextEdit(options => { });

    #region Auth

    // AD B2C
    //builder.Services
    //  .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    //  .AddMicrosoftIdentityWebApp(options =>
    //{
    //  builder.Configuration.Bind("AzureAdB2C", options);
    //});
    builder.Services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
      options.Authority = "http://host.docker.internal:8080/auth/realms/master";
      options.RequireHttpsMetadata = builder.Environment.IsDevelopment() ? false : true;
      options.ClientId = "thedashboard-frontend";
      options.ClientSecret = "viDCW0m9PeFcidApMyd9qb5y5mBf913i";
      options.ResponseType = "code";
      options.SaveTokens = true;
      options.GetClaimsFromUserInfoEndpoint = true;     
    });

    // Enable Pii



    builder.Services.AddAuthorization();

    #endregion Auth

    #region Hub
    builder.Services.AddSingleton<ITileDataService, TileDataService>();
    builder.Services.AddSingleton<IDashboardService, DashboardService>();
    builder.Services.AddSingleton<IDataSourceService, DataSourceService>();
    #endregion

    // Services that address the microservices, using httpclient to route through the proxy
    // TODO: Add auth
    builder.Services.AddSingleton<IDashboardClient>(sp =>
    {
      var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("HttpQueryProxy");
      return new DashboardClient(builder.Configuration["QueryServices:BaseUrl"], httpClient);
    });
    builder.Services.AddSingleton<ITilesClient>(sp =>
    {
      var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("HttpQueryProxy");
      return new TilesClient(builder.Configuration["QueryServices:BaseUrl"], httpClient);
    });
    builder.Services.AddSingleton<IDataSourceClient>(sp =>
    {
      var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("HttpQueryProxy");
      return new DataSourceClient(builder.Configuration["QueryServices:BaseUrl"], httpClient);
    });
    builder.Services.AddSingleton<IUiInfoClient>(sp =>
    {
      var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("HttpQueryProxy");
      return new UiInfoClient(builder.Configuration["QueryServices:BaseUrl"], httpClient);
    });

    builder.Services.AddAutoMapper(typeof(ModelMappings).Assembly);

    builder.Services.AddResponseCompression(opts =>
    {
      opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "application/octet-stream" });
    });

    var app = builder.Build();

    // TODO: Add and configure Prometheus
    //app.UseMetricServer();
    //app.UseHttpMetrics();

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
    app.UseWebOptimizer();
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
      endpoints.MapBlazorHub().AllowAnonymous().RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = $"{OpenIdConnectDefaults.AuthenticationScheme}" });
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