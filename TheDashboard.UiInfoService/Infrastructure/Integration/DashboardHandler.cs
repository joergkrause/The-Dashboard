using MassTransit;
using TheDashboard.UiInfoService.Hubs;
using TheDashboard.UiInfoService.Infrastructure.Integration.Events;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public class DashboardCreatedHandler : IConsumer<DashboardCreatedEvent>
{

  private readonly InfoHub _infoHub;
  
  public DashboardCreatedHandler(InfoHub infoHub)
  {
    _infoHub = infoHub;
  }


  public async Task Consume(ConsumeContext<DashboardCreatedEvent> context)
  {
    await _infoHub.SendMessage(1, context.Message.Name);
  }
}
