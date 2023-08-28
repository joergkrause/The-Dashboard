using AutoMapper;
using MassTransit;
using Quartz;
using TheDashboard.DataSourceService.BusinessLogic;
using TheDashboard.DataSourceService.Jobs;
using TheDashboard.SharedEntities;

namespace TheDashboard.DataSourceService.Infrastructure.Integration;

public class JobStartHandler : IConsumer<JobStartEvent>
{

  private readonly IMapper _mapper;
  private readonly IDataSourceService _dataService;
  private readonly ISchedulerFactory _schedulerFactory;

  public JobStartHandler(IMapper mapper, IDataSourceService dataService, ISchedulerFactory schedulerFactory)
  {
    _mapper = mapper;
    _dataService = dataService;
    _schedulerFactory = schedulerFactory;
  }


  public async Task Consume(ConsumeContext<JobStartEvent> context)
  {
    var scheduler = await _schedulerFactory.GetScheduler();
    var job = JobBuilder.Create<ConsumerJob>().Build();
    var consumer = await _dataService.GetDataSource(context.Message.ConsumerId);

    // use consumer to configure
    var trigger = TriggerBuilder.Create()
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(10)
            .RepeatForever())
        .Build();

    await scheduler.ScheduleJob(job, trigger);
  }
}
