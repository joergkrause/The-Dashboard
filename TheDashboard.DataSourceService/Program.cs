using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Diagnostics;
using TheDashboard.BuildingBlocks.Extensions;
using TheDashboard.DatabaseLayer.Extensions;
using TheDashboard.DataSourceService.BusinessLogic;
using TheDashboard.DataSourceService.BusinessLogic.MappingProfiles;
using TheDashboard.DataSourceService.Controllers.Implementation;
using TheDashboard.DataSourceService.Domain;
using TheDashboard.DataSourceService.Infrastructure;
using TheDashboard.DataSourceService.Infrastructure.Integration;
using TheDashboard.DataSourceService.Jobs;
using TheDashboard.SharedEntities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultServices();

var cs = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataSourceDbContext>(opt =>
{
  opt.LogTo(s => Debug.WriteLine(s), LogLevel.Warning);
  opt.UseSqlServer(cs);
});

builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDataSourceService, DataSourceService>();
builder.Services.AddScoped<DashboardCreatedHandler>();
builder.Services.AddScoped<DashboardUpdatedHandler>();
builder.Services.AddScoped<DashboardRemovedHandler>();
builder.Services.AddScoped<DataSourceCreatedHandler>();

builder.Services.AddScoped<IDataSourceBaseController, DataSourceControllerImpl>();

builder.Services.AddEventbus<DataSourceDbContext, DashboardCreatedHandler>(builder.Configuration, nameof(DataSourceService));
// add eventbus channel for direct publish

builder.Services.AddQuartz(config =>
{
  // config.UseMicrosoftDependencyInjectionJobFactory();

  // configure persistence with EF Core
  config.UsePersistentStore(opt =>
  {    
    opt.UseProperties = true;
    opt.RetryInterval = TimeSpan.FromSeconds(15);
    opt.UseClustering();
    opt.UseSqlServer(ado => {
      ado.TablePrefix = "QRTZ_";
      ado.ConnectionString = cs!;      
    });
    opt.UseJsonSerializer();
  });

  var consumerKey = new JobKey("ConsumerJob");
  config.AddJob<ConsumerJob>(opt => opt.WithIdentity(consumerKey));

  // heartbeat job
  config.AddTrigger(opt => opt
     .ForJob(consumerKey)
        .WithIdentity("ConsumerJobTrigger")
           .WithCronSchedule("0/5 * * * * ?"));
});

builder.Services.AddTransient<ConsumerJob>();

builder.Services.AddQuartzHostedService(config =>
{
  config.WaitForJobsToComplete = true;
});

builder.Services.AddHealthChecks();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.MapControllers();
app.UseHttpsRedirection();

await app.ExecuteMigration<DataSourceDbContext, Dashboard, Guid>(async (ctx, _) => await SeedDatabase.Seed(ctx));

app.Run();

