using AutoMapper;
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
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();

builder.Services.AddLogging(config => config.AddConsole());

var cs = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DashboardContext>(options =>
{
  options.UseSqlServer(cs);
});

Console.WriteLine("******************************* Setup Configurations");
// get logger
var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
logger?.LogDebug("******************************* Setup Configurations");

foreach (var type in typeof(DashboardContext).Assembly.DefinedTypes
            .Where(t => !t.IsAbstract
                        && !t.IsGenericTypeDefinition
                        && typeof(EntityTypeConfigurationDependency).IsAssignableFrom(t)))
{
  Console.WriteLine("******************************* Configure " + type.Name);
  logger?.LogDebug("******************************* Configure " + type.Name);
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

app.UseDefaultConfiguration();
await ApplyMigration();
app.Run();

async Task ApplyMigration()
{
  using var scope = app.Services.CreateScope();
  var context = scope.ServiceProvider.GetRequiredService<DashboardContext>();
  bool newDatabase = !context.Database.GetService<IRelationalDatabaseCreator>().Exists();
  await context.Database.MigrateAsync();
  var hasData = await context.Set<Dashboard>().AnyAsync();
  if (newDatabase || !hasData)
  {
    await SeedDatabase.Seed(context);
  }
  context.Dispose();
}
