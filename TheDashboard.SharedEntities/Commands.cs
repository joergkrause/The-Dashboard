namespace TheDashboard.SharedEntities;

// Commands are being sent from the frontend to the queue and being consumed by one of the backend services.
// Commands are supposed to have ONE endpoint only.

public record AddDashboard(Guid Id, DashboardDto Item) : Command;

public record RemoveDashboard(Guid Id) : Command;

public record UpdateDashboard(Guid Id, DashboardDto Item) : Command;

public record AddTile(int Id, TileDto Item) : Command;

public record RemoveTile(int TileId) : Command;

public record UpdateTile(int TileId, TileDto Item) : Command;

public record AssigneTile(int TileId, Guid DashboardId) : Command;

public record UnAssigneTile(int TileId, Guid DashboardId) : Command;

public record AddDataSource(int DataSourceId, DataSourceDto Item) : Command;

public record RemoveDataSource(int DataSourceId, int TileId) : Command;

public record UpdateDataSource(int DataSourceId, DataSourceDto Item) : Command;

public record AssignDataSource(int DataSourceId, int TileId) : Command;

public record UnAssignDataSource(int DataSourceId, int TileId) : Command;
