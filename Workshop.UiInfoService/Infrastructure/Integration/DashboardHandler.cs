using MassTransit;
using Workshop.UiInfoService.Hubs;
using Workshop.UiInfoService.Infrastructure.Integration.Events;

namespace Workshop.UiInfoService.Infrastructure.Integration;

public class DashboardCreatedHandler : IConsumer<DashboardCreatedEvent>
{

  private readonly InfoHub _infoHub;
  
  public DashboardCreatedHandler(InfoHub infoHub)
  {
    _infoHub = infoHub;
  }


  public async Task Consume(ConsumeContext<DashboardCreatedEvent> context)
  {
    await _infoHub.SendMessage("", context.Message.Name);
  }
}
