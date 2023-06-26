using Microsoft.EntityFrameworkCore;
using TheDashboard.DatabaseLayer;
using TheDashboard.TileService.Domain;

namespace TheDashboard.TileService.Infrastructure;

public class TileRepository : GenericRepository<TileDbContext, Tile, int>
{

  public TileRepository(TileDbContext context) : base(context)
  {
  }

  public async Task<IEnumerable<Tile>> GetTilesForDashboard(Guid dashboardId)
  {
    var tiles = await Context.Set<Tile>().Where(t => t.Dashboard.Id == dashboardId).ToListAsync();
    return tiles;
  }

}
