using MassTransit;
using Quartz;
using TheDashboard.DataSourceService.BusinessLogic;
using TheDashboard.DataSourceService.Infrastructure;
using TheDashboard.SharedEntities;
using static MassTransit.Logging.DiagnosticHeaders.Messaging;

namespace TheDashboard.DataSourceService.Jobs;

/// <summary>
/// Each tile connected to a data consumer may create an instance of the consumer job.
/// If the tile becomes visible the consumer starts collecting data.
/// The data is then queued for processing and the ui is updated through a signal r (websocket) message.
/// A RabbitMQ fanout could be used to parallelize the processing for other instances.
/// </summary>
public class ConsumerJob : IJob
{
  private readonly ILogger? _logger;
  private readonly IDataSourceService _dataSourceService;
  private readonly IPublishEndpoint _publishEndpoint;
  private readonly DataConsumerDbContext _dbConsumerContext;
  private readonly IBusControl _bus;

  public ConsumerJob(ILogger<ConsumerJob> logger, IConfiguration configuration, IDataSourceService dataSourceService, IPublishEndpoint publishEndpoint, DataConsumerDbContext dbConsumerContext)
  {
    _logger = logger;
    _dataSourceService = dataSourceService;
    _publishEndpoint = publishEndpoint;
    _dbConsumerContext = dbConsumerContext;
    _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
      cfg.Host($"rabbitmq://{configuration["rabbitmq:Host"]}", h =>
      {
        h.Username(configuration["rabbitmq:User"]);
        h.Password(configuration["rabbitmq:Password"]);
      });
    });
    // try connection before continue
    try
    {
      _bus.Start();
      _bus.Stop();
    }
    catch (Exception ex)
    {
      _logger?.LogError(ex.Message);
    }
  }

  public async Task Execute(IJobExecutionContext context)
  {
    var map = context.MergedJobDataMap;
    await _bus.StartAsync();
    try
    {
      var consumerId = map.GetInt("dataConsumerId");
      if (consumerId == 0)
      {
        // if no id is sent we assume this is the heartbeat job
        await _publishEndpoint.Publish(new DataEvent(DateTime.Now.ToLongTimeString()));
        await _dbConsumerContext.SaveChangesAsync();
        return;
      }

      // get task data
      var source = await _dataSourceService.GetDataSource(consumerId);
      if (source == null) return;

      // execute desired action
      var url = source.Url;
      var client = new HttpClient();
      var response = await client.GetStringAsync(url);
      // publish result to queue      
      await _publishEndpoint.Publish(new DataEvent(response));
      await _dbConsumerContext.SaveChangesAsync();
    }
    catch (Exception ex)
    {
      _logger?.LogError(ex.Message);
      throw;
    }
    finally
    {
      await _bus.StopAsync();
    }
  }
}
