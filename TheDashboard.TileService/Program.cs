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
using TheDashboard.SharedEntities;
using TheDashboard.TileService.Controllers.Implementation;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();
builder.Services.AddDbContext<TileDbContext>(opt =>
{
  opt.LogTo(s => Debug.WriteLine(s), LogLevel.Warning);
  opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!);
});

builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ITileService, TileService>();
builder.Services.AddScoped<DashboardAddedHandler>();
builder.Services.AddScoped<DashboardUpdatedHandler>();
builder.Services.AddScoped<DashboardRemovedHandler>();

builder.Services.AddScoped<ITileBaseController, TileControllerImpl>();

builder.Services.AddEventbus<TileDbContext>(builder.Configuration, nameof(TileService));

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
await app.ExecuteMigration<TileDbContext, Dashboard, Guid>(async (ctx, _) => await SeedDatabase.Seed(ctx));

app.Run();
