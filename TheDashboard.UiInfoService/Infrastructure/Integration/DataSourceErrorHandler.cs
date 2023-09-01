using MassTransit;
using Microsoft.AspNetCore.SignalR;
using TheDashboard.SharedEntities;
using TheDashboard.UiInfoService.Hubs;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public class DataSourceErrorHandler : IConsumer<DataSourceOperationError>
{

  private readonly IHubContext<InfoHub> _infoHub;

  public DataSourceErrorHandler(IHubContext<InfoHub> infoHub)
  {
    _infoHub = infoHub;
  }

  public async Task Consume(ConsumeContext<DataSourceOperationError> context)
  {
    await _infoHub.Clients.All.SendAsync("DataError", context.Message.OperationKind, context.Message.UserMessage);
  }
}
