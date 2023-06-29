using Microsoft.AspNetCore.SignalR;

namespace TheDashboard.UiInfoService.Hubs;

public class InfoHub : Hub
{


  public async Task SendMessage(int tileId, string message)
  {
    await Clients.All.SendAsync("DataUpdate", tileId, message);
  }

  public async Task SendGroupMessage(int tileId, string message)
  {
    await Clients.Group("SignalR Users").SendAsync("DataUpdate", tileId, message);
  }

  public override async Task OnConnectedAsync()
  {

    await base.OnConnectedAsync();
  }

}
