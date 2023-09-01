using MassTransit;
using Microsoft.AspNetCore.SignalR;
using TheDashboard.SharedEntities;
using TheDashboard.UiInfoService.Hubs;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public class DashboardErrorHandler : IConsumer<DashboardOperationError>
{

  private readonly IHubContext<InfoHub> _infoHub;

  public DashboardErrorHandler(IHubContext<InfoHub> infoHub)
  {
    _infoHub = infoHub;
  }


  public async Task Consume(ConsumeContext<DashboardOperationError> context)
  {
    await _infoHub.Clients.All.SendAsync("DataError", context.Message.OperationKind, context.Message.UserMessage);
  }
}
