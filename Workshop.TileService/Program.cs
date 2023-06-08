using Workshop.BuildingBlocks.Extensions;
using Workshop.TileService.BusinessLogic;
using Workshop.TileService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEventbus<TileDbContext>(builder.Configuration, nameof(TileService));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// ExecuteMigrations();
app.Run();
