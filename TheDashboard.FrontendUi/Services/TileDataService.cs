using Microsoft.AspNetCore.SignalR.Client;
using System.Runtime.CompilerServices;

namespace TheDashboard.FrontendUi.Services;

public delegate void OnMessageEvent(int tileId, string message);

public class TileDataService : ITileDataService
{

  private readonly ILogger<TileDataService> _logger;
  private readonly IConfiguration _configuration;
  private HubConnection? hubConnection;

  public TileDataService(ILogger<TileDataService> logger, IConfiguration configuration)
  {
    _logger = logger;
    _configuration = configuration;

    hubConnection = new HubConnectionBuilder().WithUrl(_configuration["HubUrl"]!).Build();

    hubConnection.On<int, string>("ReceiveData", (tileId, message) =>
    {
      OnMessage(tileId, message);
    });

  }

  public async Task Init()
  {
    if (hubConnection == null)
    {
      throw new ArgumentNullException("hubConnection not set");
    }
    await hubConnection.StartAsync();
  }

  public event OnMessageEvent Message;

  private void OnMessage(int tileId, string message)
  {
    Message?.Invoke(tileId, message);
  }

  public bool IsConnected { get => hubConnection?.State == HubConnectionState.Connected; }

}
