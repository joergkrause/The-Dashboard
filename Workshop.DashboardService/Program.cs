using Microsoft.EntityFrameworkCore;
using Workshop.BuildingBlocks.Extensions;
using Workshop.DashboardService.Infrastructure;
using Workshop.Services;
using Workshop.Services.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();

builder.Services.AddDbContext<DashboardContext>(opt =>
{
  opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IDashboardService, DashboardService>();
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

app.Run();
