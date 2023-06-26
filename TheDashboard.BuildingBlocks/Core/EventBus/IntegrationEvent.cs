using MassTransit;

namespace TheDashboard.BuildingBlocks.Core.EventBus;

public record IntegrationEvent : CorrelatedBy<Guid>
{
  public Guid CorrelationId { get; } = Guid.NewGuid();
}
