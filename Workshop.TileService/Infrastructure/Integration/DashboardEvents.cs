using Workshop.BuildingBlocks.Core.EventBus;
using Workshop.Services.TransferObjects;

namespace Workshop.TileService.Infrastructure.Integration.Events;

public record DashboardCreatedEvent(Guid Id, string Name) : IntegrationEvent;

public record DashboardDeletedEvent(Guid Id) : IntegrationEvent;

public record DashboardUpdatedEvent(Guid Id, string Name) : IntegrationEvent;


// InsertOrUpdate
