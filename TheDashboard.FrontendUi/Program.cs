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
using TheDashboard.FrontendUi.Hubs;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web;
using Blazorise.RichTextEdit;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;
using TheDashboard.FrontendUi.Services.Mapper;
using TheDashboard.FrontendUi.Services;

namespace TheDashboard.FrontendUi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      builder.Logging.AddAzureWebAppDiagnostics(configure =>
      {
        configure.IncludeScopes = true;
      });

      builder.Services.AddAzureAppConfiguration();
      builder.Services.AddFeatureManagement().AddFeatureFilter<TimeWindowFilter>();

      builder.Configuration.AddAzureAppConfiguration(options =>
      {
        var appConfConnectionString = builder.Configuration.GetConnectionString("AppConfig");
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

      builder.Services.AddResponseCaching();
      builder.Services.AddControllers();
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

      builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    // for B2C service
    .AddMicrosoftIdentityWebApp(options =>
    {
      builder.Configuration.Bind("AzureAdB2C", options);
      options.TokenValidationParameters.ValidateIssuerSigningKey = false;
      options.TokenValidationParameters.ValidateIssuer = false;
      options.Events ??= new OpenIdConnectEvents();
      options.Events.OnRemoteFailure += async (context) =>
      {
        context.HandleResponse();
        if (context.Failure is OpenIdConnectProtocolException protocolException)
        {
          var errors = protocolException.Data["error_description"].ToString().Split(Environment.NewLine);
          context.Response.Redirect("/Public/Error?message=" + String.Join(", ", errors));
          context.HandleResponse();
          await Task.FromResult(0);
        }
        await Task.CompletedTask.ConfigureAwait(false);
      };
      options.Events.OnMessageReceived += async (context) =>
      {
        Debug.WriteLine(context.Token);
        await Task.CompletedTask.ConfigureAwait(false);
      };
      options.Events.OnSignedOutCallbackRedirect += async (context) =>
      {
        context.HttpContext.Response.Redirect("/");
        await Task.CompletedTask.ConfigureAwait(false);
      };
      options.Events.OnTokenResponseReceived += async (TokenResponseReceivedContext context) =>
      {
        Debug.WriteLine(context.TokenEndpointResponse);
        await Task.CompletedTask.ConfigureAwait(false);
      };
      options.Events.OnTokenValidated += async (context) =>
      {
        await Task.CompletedTask.ConfigureAwait(false);
      };
      options.Events.OnTicketReceived += async (TicketReceivedContext context) =>
      {
        var tenantId = context.Principal?.Claims.Single(c => c.Type == "http://schemas.microsoft.com/identity/claims/tenantid").Value;
        context.ReturnUri = "/Portal";
        await Task.CompletedTask.ConfigureAwait(false);
      };
      options.SaveTokens = true;
    });

      builder.Services.AddSingleton<IDashboardClient, DashboardClient>();
      builder.Services.AddSingleton<ITilesClient, TilesClient>();
      builder.Services.AddSingleton<IDataConsumerClient, DataConsumerClient>();

      builder.Services.AddSingleton<IDashboardViewerService, DashboardViewerService>();

      builder.Services.AddAutoMapper(typeof(ModelMappings).Assembly);

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

      app.UseCors();
      app.UseHttpsRedirection();
      if (!app.Environment.IsDevelopment())
      {
        app.UseResponseCompression();
        app.UseExceptionHandler("/Error");
        app.UseHsts();
      }
      app.UseStaticFiles();

      app.UseRouting();

      app.MapBlazorHub();
      app.MapFallbackToPage("/_Host");

      app.Run();
    }
  }
}