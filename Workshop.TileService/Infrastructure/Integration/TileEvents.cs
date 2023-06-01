using Workshop.BuildingBlocks.Core.EventBus;
using Workshop.Services.TransferObjects;

// ????

namespace Workshop.TileService.Infrastructure.Integration.Events;

public record TileCreatedEvent(Guid Id, Guid DashboardId, string Name)  : IntegrationEvent;

public record TileDeletedEvent(Guid Id): IntegrationEvent;

public record TileUpdatedEvent(Guid Id, string Name): IntegrationEvent;


// InsertOrUpdate
