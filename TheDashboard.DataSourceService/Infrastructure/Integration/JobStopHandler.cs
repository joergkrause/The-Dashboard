using MassTransit;
using Quartz;
using TheDashboard.SharedEntities;

namespace TheDashboard.DataSourceService.Infrastructure.Integration;

public class JobStopHandler : IConsumer<JobStopEvent>
{

  private readonly ISchedulerFactory _schedulerFactory;

  public JobStopHandler(ISchedulerFactory schedulerFactory)
  {
    _schedulerFactory = schedulerFactory;
  }


  public async Task Consume(ConsumeContext<JobStopEvent> context)
  {
    var scheduler = await _schedulerFactory.GetScheduler();
    await scheduler.Clear();
  }
}