using TheDashboard.DatabaseLayer.Extensions;
using Microsoft.EntityFrameworkCore;
using TheDashboard.BuildingBlocks.Extensions;
using TheDashboard.DashboardService.Infrastructure;
using TheDashboard.Services;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using TheDashboard.Services.Mappings;
using TheDashboard.DashboardService.Domain;
using TheDashboard.DatabaseLayer.Interfaces;
using TheDashboard.DatabaseLayer.Interceptors;
using TheDashboard.DatabaseLayer.Configurations;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();
builder.Services.AddLogging(config => config.AddConsole());

var cs = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DashboardContext>(options =>
{
  options.UseSqlServer(cs);
});

// get logger
// var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();

foreach (var type in typeof(DashboardContext).Assembly.DefinedTypes
            .Where(t => !t.IsAbstract
                        && !t.IsGenericTypeDefinition
                        && typeof(EntityTypeConfigurationDependency).IsAssignableFrom(t)))
{
  builder.Services.AddSingleton(typeof(EntityTypeConfigurationDependency), type);
}

builder.Services.AddSingleton<IEncryptionService, EncryptionService>();

builder.Services.AddScoped<IDateTime, CurrentDateTime>();
builder.Services.AddScoped<IUser, CurrentUser>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ILayoutService, LayoutService>();

builder.Services.AddEventbus<DashboardContext>(builder.Configuration, nameof(DashboardService));

builder.Services.AddSwaggerGen(config =>
{
  config.SwaggerDoc("v1", new() { Title = "Dashboard API", Version = "v1" });
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
  config.SwaggerEndpoint("/swagger/v1/swagger.json", "Dashboard API v1");  
});
app.MapControllers();
app.UseDefaultConfiguration();

await app.ExecuteMigration<DashboardContext, Dashboard, Guid>(async (ctx, _) => await SeedDatabase.Seed(ctx));

app.Run();

