using TheDashboard.DatabaseLayer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using TheDashboard.BuildingBlocks.Extensions;
using TheDashboard.TileService.BusinessLogic;
using TheDashboard.TileService.BusinessLogic.MappingProfiles;
using TheDashboard.TileService.Domain;
using TheDashboard.TileService.Infrastructure;
using TheDashboard.TileService.Infrastructure.Integration;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();

builder.Services.AddDbContext<TileDbContext>(opt =>
{
  opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!);
});

builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ITileService, TileService>();
builder.Services.AddScoped<DashboardCreatedHandler>();
builder.Services.AddScoped<DashboardUpdatedHandler>();
builder.Services.AddScoped<DashboardRemovedHandler>();

builder.Services.AddEventbus<TileDbContext>(builder.Configuration, nameof(TileService));

builder.Services.AddSwaggerGen(config =>
{
  config.SwaggerDoc("v1", new() { Title = "Tiles API", Version = "v1" });
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
  config.SwaggerEndpoint("/swagger/v1/swagger.json", "Tiles API v1");  
});

app.UseHttpsRedirection();
app.MapControllers();
await app.ExecuteMigration<TileDbContext, Dashboard, Guid>(async (ctx, _) => await SeedDatabase.Seed(ctx));

app.Run();
