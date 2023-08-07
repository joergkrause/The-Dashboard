namespace TheDashboard.SharedEntities;

public record DashboardAdded(Guid Id, DashboardDto Item) : Command;

public record DashboardRemoved(Guid Id) : Command;

public record DashboardUpdated(Guid Id, DashboardDto Item) : Command;

public record TileAdded(int Id, TileDto Item) : Command;

public record TileRemoved(int TileId) : Command;

public record TileUpdated(int TileId, TileDto Item) : Command;

public record TileAssigned(int TileId, Guid DashboardId) : Command;

public record TileUnAssigned(int TileId, Guid DashboardId) : Command;

public record DataSourceAdded(int DataSourceId, DataSourceDto Item) : Command;

public record DataSourceRemoved(int DataSourceId, int TileId) : Command;

public record DataSourceUpdated(int DataSourceId, DataSourceDto Item) : Command;

public record DataSourceAssigned(int DataSourceId, int TileId) : Command;

public record DataSourceUnAssigned(int DataSourceId, int TileId) : Command;
