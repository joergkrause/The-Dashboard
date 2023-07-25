using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using TheDashboard.DatabaseLayer.Domain.Contracts;
using TheDashboard.DataConsumerService.Infrastructure.Integration;
using TheDashboard.UiInfoService.Hubs;
using TheDashboard.UiInfoService.Infrastructure.Integration.Models;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public class ConsumerHandler<T> : IConsumer<DataEvent> where T : DataConsumerMessage
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
