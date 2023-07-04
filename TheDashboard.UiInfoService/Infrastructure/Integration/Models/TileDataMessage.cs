namespace TheDashboard.UiInfoService.Infrastructure.Integration.Models;

public interface ITileDataMessage
{
  Task SendTileData(TileData data);
}
