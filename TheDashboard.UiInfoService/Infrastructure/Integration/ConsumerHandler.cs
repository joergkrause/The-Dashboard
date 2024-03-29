﻿using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using TheDashboard.SharedEntities;
using TheDashboard.UiInfoService.Hubs;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public class ConsumerHandler : IConsumer<DataEvent>
{

  private readonly IHubContext<InfoHub> _infoHub;
  
  public ConsumerHandler(IHubContext<InfoHub> infoHub)
  {
    _infoHub = infoHub;
  }

  public async Task Consume(ConsumeContext<DataEvent> context)
  {
    var tileId = context.Message.CorrelationId;
    await _infoHub.Clients.All.SendAsync("SendTileData", tileId, context.Message.Data);
  }

  public async Task ConsumeTest(DataEvent @event)
  {
    var tileId = @event.CorrelationId;
    await _infoHub.Clients.All.SendAsync("SendTileData", tileId, JsonSerializer.Serialize(@event.Data));
  }
}
