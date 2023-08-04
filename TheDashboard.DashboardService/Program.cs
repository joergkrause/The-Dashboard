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
using TheDashboard.SharedEntities;
using TheDashboard.DashboardService.Controllers.Implementation;

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

// the generated controllers request this implementation to execute actual requests
builder.Services.AddScoped<IDashboardBaseController, DashboardControllerImpl>();

builder.Services.AddEventbus<DashboardContext>(builder.Configuration, nameof(DashboardService));

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.MapControllers();
app.UseDefaultConfiguration();

await app.ExecuteMigration<DashboardContext, Dashboard, Guid>(async (ctx, _) => await SeedDatabase.Seed(ctx));

app.Run();

