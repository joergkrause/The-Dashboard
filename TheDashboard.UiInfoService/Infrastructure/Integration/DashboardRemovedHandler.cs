using MassTransit;
using Microsoft.AspNetCore.SignalR;
using TheDashboard.SharedEntities;
using TheDashboard.UiInfoService.Hubs;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public class DashboardRemovedHandler : IConsumer<DashboardRemoved>
{

  private readonly IHubContext<InfoHub> _infoHub;

  public DashboardRemovedHandler(IHubContext<InfoHub> infoHub)
  {
    _infoHub = infoHub;
  }


  public async Task Consume(ConsumeContext<DashboardRemoved> context)
  {
    await _infoHub.Clients.All.SendAsync("DataUpdated");
  }
}
