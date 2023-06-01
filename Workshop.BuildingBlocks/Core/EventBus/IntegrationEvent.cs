using MassTransit;

namespace Workshop.BuildingBlocks.Core.EventBus;

public record IntegrationEvent : CorrelatedBy<Guid>
{
  public Guid CorrelationId { get; } = Guid.NewGuid();
}
