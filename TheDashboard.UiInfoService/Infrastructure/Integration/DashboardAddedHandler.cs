using MassTransit;
using Microsoft.AspNetCore.SignalR;
using TheDashboard.SharedEntities;
using TheDashboard.UiInfoService.Hubs;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public class DashboardAddedHandler : IConsumer<DashboardAdded>
{

  private readonly IHubContext<InfoHub> _infoHub;

  public DashboardAddedHandler(IHubContext<InfoHub> infoHub)
  {
    _infoHub = infoHub;
  }


  public async Task Consume(ConsumeContext<DashboardAdded> context)
  {
    await _infoHub.Clients.All.SendAsync("DataUpdated");
  }
}
