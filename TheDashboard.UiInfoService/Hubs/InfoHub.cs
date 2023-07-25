using Microsoft.AspNetCore.SignalR;
using TheDashboard.UiInfoService.Infrastructure.Integration.Models;

namespace TheDashboard.UiInfoService.Hubs;

public class InfoHub : Hub<ITileDataMessage>
{

  public override Task OnConnectedAsync()
  {
    return base.OnConnectedAsync();
  }

  public override Task OnDisconnectedAsync(Exception? exception)
  {
    return base.OnDisconnectedAsync(exception);
  }

  
}
