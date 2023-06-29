using Microsoft.AspNetCore.SignalR;

namespace TheDashboard.UiInfoService.Hubs;

public class InfoHub : Hub
{


  public async Task SendMessage(string user, string message)
  {
    await Clients.All.SendAsync("DataUpdate", user, message);
  }

  public async Task SendGroupMessage(string user, string message)
  {
    await Clients.Group("SignalR Users").SendAsync("DataUpdate", user, message);
  }

  public override async Task OnConnectedAsync()
  {

    await base.OnConnectedAsync();
  }

}
