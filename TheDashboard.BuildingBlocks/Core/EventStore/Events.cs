namespace TheDashboard.BuildingBlocks.Core.EventStore;

public record DashboardAdded(Guid Id, string Name): Command;

public record DashboardRemoved(Guid Id) : Command;

public record DashboardUpdated(Guid Id, string Name) : Command;
