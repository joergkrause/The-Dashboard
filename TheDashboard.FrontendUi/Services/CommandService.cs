using EventStore.Client;
using System.Text.Json;
using System.Threading;
using TheDashboard.Clients;

namespace TheDashboard.FrontendUi.Services;

/// <summary>
/// Follow the CQRS pattern, all user actions are commands and go into the command service and then into event store.
/// The event store is the single source of truth for the application. Any changes trigger messages that are sent to the bus and any service can consume this.
/// </summary>
public class CommandService
{

  private readonly EventStoreClient _client;

  public CommandService(EventStoreClient client)
  {
    _client = client;
  }


  public async Task AddDashboard(CancellationToken cancellationToken)
  {
    var evt = new DashboardDto { Id = Guid.NewGuid() };

    var eventData = new EventData(
        Uuid.NewUuid(),
        "TestEvent",
        JsonSerializer.SerializeToUtf8Bytes(evt)
    );
    await _client.AppendToStreamAsync("some-stream", StreamState.Any, new[] { eventData }, cancellationToken: cancellationToken);
  }

  public void UpdateDashboard()
  {
    throw new NotImplementedException();
  }

  public void DeleteDashboard()
  {
    throw new NotImplementedException();
  }

  public void AddTile()
  {
    throw new NotImplementedException();

  }

  public void UpdateTile()
  {
    throw new NotImplementedException();

  }

  public void DeleteTile()
  {
    throw new NotImplementedException();

  }
}
