using Microsoft.AspNetCore.SignalR.Client;
using TheDashboard.SharedEntities;

namespace TheDashboard.Frontend.Services;

public delegate void OnMessageEvent(Guid tileId, string message);

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

    // listen to the hub from uiservice
    hubConnection.On<Guid, string>("SendTileData", (tileId, message) =>
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
    var retryCount = 0;
    var maxRetryAttempts = 12; // You can change this to fit your needs
    var delay = TimeSpan.FromSeconds(5);

    while (retryCount < maxRetryAttempts)
    {
      try
      {
        await hubConnection.StartAsync();
        break; // connection is established, exit the loop
      }
      catch (Exception ex)
      {
        _logger?.LogWarning(ex.Message);
        // Handle exception here. It's often useful to log this error

        retryCount++;
        await Task.Delay(delay);
      }
    }

    if (retryCount == maxRetryAttempts)
    {
      // Handle the scenario when the connection was not established within the given attempts and delay.
    }
  }

  public event OnMessageEvent Message;

  private void OnMessage(Guid tileId, string message)
  {
    Message?.Invoke(tileId, message);
  }

    public Task InvokeCommand<TEvent>(TileDto dto) where TEvent : Command
    {
        throw new NotImplementedException();
    }

    public bool IsConnected { get => hubConnection?.State == HubConnectionState.Connected; }

}
