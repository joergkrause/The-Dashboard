using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using TheDashboard.UiInfoService.Hubs;
using TheDashboard.UiInfoService.Infrastructure.Integration.Models;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public class ConsumerHandler<T> : IConsumer<ConsumerEvent<T>> where T : class
{

  private readonly IHubContext<InfoHub> _infoHub;
  
  public ConsumerHandler(IHubContext<InfoHub> infoHub)
  {
    _infoHub = infoHub;
  }

  public async Task Consume(ConsumeContext<ConsumerEvent<T>> context)
  {
    var tileId = context.Message.TileId;
    await _infoHub.Clients.All.SendAsync(nameof(ITileDataMessage.SendTileData), tileId, context.Message.Data);
  }

  public async Task ConsumeTest(ConsumerEvent<T> @event)
  {
    var tileId = @event.TileId;
    await _infoHub.Clients.All.SendAsync(nameof(ITileDataMessage.SendTileData), tileId, JsonSerializer.Serialize(@event.Data));
  }
}
