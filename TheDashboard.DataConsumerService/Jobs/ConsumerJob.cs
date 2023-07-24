using MassTransit;
using Quartz;
using TheDashboard.DataConsumerService.BusinessLogic;

namespace TheDashboard.DataConsumerService.Jobs;

/// <summary>
/// Each tile connected to a data consumer may create an instance of the consumer job.
/// If the tile becomes visible the consumer starts collecting data.
/// The data is then queued for processing and the ui is updated through a signal r (websocket) message.
/// A RabbitMQ fanout could be used to parallelize the processing for other instances.
/// </summary>
public class ConsumerJob : IJob
{

  private readonly IDataConsumerService _dataConsumerService;
  private readonly IPublishEndpoint _publishEndpoint;

  public ConsumerJob(IDataConsumerService dataConsumerService, IPublishEndpoint publishEndpoint)
  {
    _dataConsumerService = dataConsumerService;
    _publishEndpoint = publishEndpoint;

  }

  public async Task Execute(IJobExecutionContext context)
  {
    var map = context.MergedJobDataMap;
    var consumerId = map.GetInt("dataConsumerId");
    if (consumerId == 0)
    {
      // if no id is sent we assume this is the heartbeat job
      await _publishEndpoint.Publish(new DataConsumerMessage { Data = DateTime.Now.ToLongTimeString() });
      return;
    }
    
    // get task data
    var source = await _dataConsumerService.GetDataSource(consumerId);
    if (source == null) return;

    // execute desired action
    var url = source.Url;
    var client = new HttpClient();    
    var response = await client.GetStringAsync(url);    
    // publish result to queue
    await _publishEndpoint.Publish(new DataConsumerMessage { Data = response });
  }
}
