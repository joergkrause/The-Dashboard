using Microsoft.AspNetCore.SignalR;

namespace Workshop.UiInfoService.Hubs;

public class InfoHub : Hub
{


  public async Task SendMessage(string user, string message)
  {
    await Clients.All.SendAsync("DataUpdate", user, message);
  }

}
