using MassTransit;
using TheDashboard.UiInfoService.Hubs;
using TheDashboard.UiInfoService.Infrastructure.Integration.Events;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public class ConsumerHandler : IConsumer<ConsumerEvent>
{

  private readonly InfoHub _infoHub;
  
  public ConsumerHandler(InfoHub infoHub)
  {
    _infoHub = infoHub;
  }


  public async Task Consume(ConsumeContext<ConsumerEvent> context)
  {
    // need tile id here
    await _infoHub.SendMessage("", context.Message.Data);
  }
}
