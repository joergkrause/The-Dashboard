using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using TheDashboard.DataConsumerService.BusinessLogic;
using TheDashboard.DataConsumerService.Jobs;
using TheDashboard.SharedEntities;

namespace TheDashboard.DataConsumerService.Infrastructure.Integration;

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