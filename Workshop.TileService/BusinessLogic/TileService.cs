using Workshop.Services.TransferObjects;
using Workshop.TileService.Infrastructure;

namespace Workshop.TileService.BusinessLogic;

public class TileService : ITileService
{
  private readonly TileDbContext _tileDbContext;

  public TileService(TileDbContext tileDbContext)
  {
    _tileDbContext = tileDbContext;
  }


  public IEnumerable<TileDto> GetAllTiles(int id)
  {
    throw new NotImplementedException();
  }

  public TileDto GetTile(int id)
  {
    throw new NotImplementedException();
  }



}
