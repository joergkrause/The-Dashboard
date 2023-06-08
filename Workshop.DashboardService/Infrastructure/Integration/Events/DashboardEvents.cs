using Workshop.BuildingBlocks.Core.EventBus;
using Workshop.Services.TransferObjects;

namespace Workshop.DashboardService.Infrastructure.Integration.Events;

public record DashboardCreatedEvent(int Id, string Name) : IntegrationEvent;

public record DashboardDeletedEvent(int Id) : IntegrationEvent;

public record DashboardUpdatedEvent(int Id, string Name) : IntegrationEvent;


// InsertOrUpdate
