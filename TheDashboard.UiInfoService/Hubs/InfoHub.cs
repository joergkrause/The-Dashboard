using Microsoft.AspNetCore.SignalR;
using TheDashboard.UiInfoService.Infrastructure.Integration.Models;

namespace TheDashboard.UiInfoService.Hubs;

public class InfoHub : Hub<ITileDataMessage>
{

  public async Task SendTileData(string user, string data) => await Clients.All.SendTileData(new TileData(user, data));

}
