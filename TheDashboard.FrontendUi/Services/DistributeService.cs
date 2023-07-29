using EventStore.Client;
using System.Threading;

namespace TheDashboard.FrontendUi.Services;

public class DistributeService
{
  private readonly EventStoreClient _client;

  public DistributeService(EventStoreClient client)
  {
    _client = client;

  }
  /// https://github.com/OKTAYKIR/EventFlow.Example
  //await client.SubscribeToStreamAsync("some-stream",
  //FromStream.Start,
  //  async (subscription, evnt, cancellationToken) => {
  //  Console.WriteLine($"Received event {evnt.OriginalEventNumber}@{evnt.OriginalStreamId}");
  //  await HandleEvent(evnt);
  //});
}
