using MassTransit;
using TheDashboard.UiInfoService.Hubs;
using TheDashboard.UiInfoService.Infrastructure.Integration.Events;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public class ConsumerHandler<T> : IConsumer<ConsumerEvent<T>> where T : class, new()
{

  private readonly InfoHub _infoHub;
  
  public ConsumerHandler(InfoHub infoHub)
  {
    _infoHub = infoHub;
  }


  public async Task Consume(ConsumeContext<ConsumerEvent<T>> context)
  {
    var tileId = context.Message.TileId;
    await _infoHub.SendMessage(tileId, context.Message.Data?.ToString()!);
  }
}
