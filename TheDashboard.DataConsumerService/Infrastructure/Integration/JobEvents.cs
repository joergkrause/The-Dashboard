using TheDashboard.BuildingBlocks.Core.EventBus;

namespace TheDashboard.DataConsumerService.Infrastructure.Integration;

public record JobStartEvent(int ConsumerId) : IntegrationEvent;

public record JobStopEvent(string Data) : IntegrationEvent;
