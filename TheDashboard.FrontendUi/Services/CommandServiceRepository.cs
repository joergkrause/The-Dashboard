using AutoMapper;
using EventStore.Client;
using MassTransit;
using System.Text.Json;
using System.Threading;
using TheDashboard.Clients;
using TheDashboard.FrontendUi.EventSourcing;

namespace TheDashboard.FrontendUi.Services;

/// <summary>
/// Follow the CQRS pattern, all user actions are commands and go into the command service and then into event store.
/// The event store is the single source of truth for the application. Any changes trigger messages that are sent to the bus and any service can consume this.
/// </summary>
public class CommandServiceRepository : ICommandServiceRepository
{
  private readonly IMapper _mapper;
  private readonly EventStoreClient _client;
  private readonly IPublishEndpoint _publishEndpoint;

  public CommandServiceRepository(IMapper mapper, EventStoreClient client, IPublishEndpoint publishEndpoint)
  {
    _mapper = mapper;
    _client = client;
    _publishEndpoint = publishEndpoint;
  }

  public async Task StoreAndPublish<TDto, TEvent>(TDto dto, CancellationToken cancellationToken)
  {
    var evt = _mapper.Map<TEvent>(dto);

    var eventData = new EventData(
        Uuid.NewUuid(),
        typeof(TEvent).Name,
        JsonSerializer.SerializeToUtf8Bytes(evt)
    );
    var result = await _client.AppendToStreamAsync("some-stream", StreamState.Any, new[] { eventData }, cancellationToken: cancellationToken);
    if (result != null)
    {
      await _publishEndpoint.Publish(evt);
    }
  }

}
