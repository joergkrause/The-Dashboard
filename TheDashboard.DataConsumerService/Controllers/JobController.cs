using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using TheDashboard.DataConsumerService.BusinessLogic;
using TheDashboard.DataConsumerService.Jobs;
using TheDatabase.DataConsumerService.Controllers;

namespace TheDashboard.DataConsumerService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobController : ControllerBase
{

  private readonly ILogger<ConsumerController>? _logger;
  private readonly IDataConsumerService _dataConsumerService;
  private readonly ISchedulerFactory _schedulerFactory;

  public JobController(IServiceProvider serviceProvider)
  {
    _logger = serviceProvider.GetService<ILogger<ConsumerController>>();
    _dataConsumerService = serviceProvider.GetRequiredService<IDataConsumerService>();
    _schedulerFactory = serviceProvider.GetRequiredService<ISchedulerFactory>();
  }

  [HttpPost("start/{consumerId:int}")]
  public async Task<IActionResult> StartJob(int consumerId)
  {
    var scheduler = await _schedulerFactory.GetScheduler();
    var job = JobBuilder.Create<ConsumerJob>().Build();
    var consumer = await _dataConsumerService.GetDataSource(consumerId);

    // use consumer to configure
    var trigger = TriggerBuilder.Create()
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(10)
            .RepeatForever())
        .Build();

    await scheduler.ScheduleJob(job, trigger);

    return Ok();
  }

  [HttpPost("stop")]
  public async Task<IActionResult> StopJob()
  {
    var scheduler = await _schedulerFactory.GetScheduler();
    await scheduler.Clear();
    return Ok();
  }

}
