using Workshop.Domain;

namespace Workshop.TileService.Infrastructure;

public class TileRepository : GenericRepository<Tile>
{

  public IEnumerable<Tile> GetTiles(int dashboardId)
  {
    return db.Set<Tile>().Where(t => t.Dashboard.Id == dashboardId);
  }

}
