using Workshop.Services.TransferObjects;

namespace Workshop.TileService.BusinessLogic
{
  public interface ITileService
  {
    IEnumerable<TileDto> GetAllTiles(int id);
    TileDto GetTile(int id);
  }
}