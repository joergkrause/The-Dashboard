using Microsoft.EntityFrameworkCore;
using TheDashboard.BuildingBlocks.Extensions;
using Workshop.DataConsumerService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();

builder.Services.AddDbContext<DataConsumerDbContext>(opt =>
{
  opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// builder.Services.AddEventbus<DataConsumerDbContext>(builder.Configuration, nameof(TileService));

builder.Services.AddSwaggerGen(config =>
{
  config.SwaggerDoc("v1", new() { Title = "Tiles API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
  config.SwaggerEndpoint("/swagger/v1/swagger.json", "Tiles API v1");  
});

app.UseHttpsRedirection();
// ExecuteMigrations();
app.Run();
