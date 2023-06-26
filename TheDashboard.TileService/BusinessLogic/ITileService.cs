using TheDashboard.TileService.Controllers.Models;

namespace TheDashboard.TileService.BusinessLogic
{
  public interface ITileService
  {
    Task<TileDto> AddTile(TileDto tileDto);
    Task DeleteTile(int id);
    Task<IEnumerable<TileDto>> GetAllTiles(Guid dashboardId);
    Task<TileDto?> GetTile(int id);
    Task<bool> HasTiles(Guid dashboardId);
    Task<TileDto> UpdateTile(TileDto tileDto);
  }
}