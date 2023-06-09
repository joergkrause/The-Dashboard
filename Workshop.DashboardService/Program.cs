using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Workshop.BuildingBlocks.Extensions;
using Workshop.DashboardService.Infrastructure;
using Workshop.Services;
using Microsoft.EntityFrameworkCore;
using Workshop.Services.Mappings;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Workshop.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();

var cs = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DashboardContext>(options =>
{
  options.UseSqlServer(cs);
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
await ApplyMigration();
app.Run();

async Task ApplyMigration()
{
  using var scope = app.Services.CreateScope();
  var context = scope.ServiceProvider.GetRequiredService<DashboardContext>();
  bool newDatabase = !context.Database.GetService<IRelationalDatabaseCreator>().Exists();
  await context.Database.MigrateAsync();
  //if (newDatabase)
  //{
  //  // Seed
  //  await SeedDatabase.Seed(context);
  //}
  var hasData = await context.Set<Dashboard>().AnyAsync();
  if (!hasData)
  {
    await SeedDatabase.Seed(context);
  }
  context.Dispose();
}
