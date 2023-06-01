using Workshop.BuildingBlocks.Extensions;
using Workshop.DashboardService.Infrastructure;
using Workshop.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();

builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddEventbus<DashboardContext>(builder.Configuration, nameof(DashboardService));

var app = builder.Build();

app.UseDefaultConfiguration();

app.Run();
