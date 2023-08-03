using MassTransit;

namespace TheDashboard.BuildingBlocks.Core.EventStore;

/// <summary>
/// Use this as a contraint for any generic command functions.
/// </summary>
public abstract record Command() : CorrelatedBy<Guid>
{
  public Guid CorrelationId { get; } = Guid.NewGuid();
}

