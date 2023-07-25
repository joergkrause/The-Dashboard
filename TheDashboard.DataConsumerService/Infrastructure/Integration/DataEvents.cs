using TheDashboard.BuildingBlocks.Core.EventBus;

namespace TheDashboard.DataConsumerService.Infrastructure.Integration;

public record DataEvent(string Data) : IntegrationEvent;
