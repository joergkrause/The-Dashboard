using Microsoft.EntityFrameworkCore;
using Workshop.BuildingBlocks.Extensions;
using Workshop.Services.Mappings;
using Workshop.TileService.BusinessLogic;
using Workshop.TileService.Infrastructure;
using Workshop.TileService.Infrastructure.Integration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();

builder.Services.AddDbContext<TileDbContext>(opt =>
{
  opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ITileService, TileService>();
builder.Services.AddScoped<DashboardCreatedHandler>();

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
// ExecuteMigrations();
app.Run();
