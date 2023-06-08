using Workshop.BuildingBlocks.Extensions;
using Workshop.DashboardService.Infrastructure;
using Workshop.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();

var cs = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DashboardContext>(options =>
{
  options.UseSqlServer(cs);
});

builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddEventbus<DashboardContext>(builder.Configuration, nameof(DashboardService));

var app = builder.Build();

app.UseDefaultConfiguration();

app.Run();
