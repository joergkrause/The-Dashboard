using Workshop.Services;
using Workshop.Services.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddSwaggerGen(config =>
{
  config.SwaggerDoc("v1", new() { Title = "Workshop API", Version = "v1" });
});

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
if (app.Environment.IsDevelopment())
{
  app.UseSwaggerUI(config =>
  {
    config.SwaggerEndpoint("/swagger/v1/swagger.json", "Workshop API v1");
    config.RoutePrefix = string.Empty;
  });
}

app.MapControllers();

app.Run();

static void Do()
{
  //
}