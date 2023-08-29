﻿using MassTransit;
using Microsoft.AspNetCore.SignalR;
using TheDashboard.SharedEntities;
using TheDashboard.UiInfoService.Hubs;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public class DashboardUpdatedHandler : IConsumer<DashboardUpdated>
{

  private readonly IHubContext<InfoHub> _infoHub;

  public DashboardUpdatedHandler(IHubContext<InfoHub> infoHub)
  {
    _infoHub = infoHub;
  }

  public async Task Consume(ConsumeContext<DashboardUpdated> context)
  {
    await _infoHub.Clients.All.SendAsync("DataUpdated");
  }
}