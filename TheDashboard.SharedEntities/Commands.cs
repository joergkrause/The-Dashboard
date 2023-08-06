namespace TheDashboard.SharedEntities;

public record DashboardAdded(Guid Id, DashboardDto Item) : Command;

public record DashboardRemoved(Guid Id) : Command;

public record DashboardUpdated(Guid Id, DashboardDto Item) : Command;

public record TileAdded(Guid DashboardId, TileDto Item) : Command;

public record TileRemoved(Guid DashboardId, int TileId) : Command;

public record TileUpdated(int TileId, TileDto Item) : Command;

