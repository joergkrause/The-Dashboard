using System.Reflection.Metadata.Ecma335;
using TheDashboard.SharedEntities;
using TheDashboard.TileService.BusinessLogic;

namespace TheDashboard.TileService.Controllers.Implementation;

public class TileControllerImpl : ITileBaseController
{

  private readonly ILogger<TileController>? _logger;
  private readonly ITileService _tileService;
  public TileControllerImpl(IServiceProvider serviceProvider)
  {
    _logger = serviceProvider.GetService<ILogger<TileController>>();
    _tileService = serviceProvider.GetRequiredService<ITileService>();
  }

  public async Task<TileDto> AddTileAsync(TileDto body)
  {
    _logger?.LogInformation("[TileController] AddTile");
    var tile = await _tileService.AddTile(body);
    return tile;
  }

  public Task<TileDto> DeleteTileAsync(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<ICollection<TileDto>> GetDashboardTilesAsync(Guid dashboardId)
  {
    _logger?.LogInformation("[TileController] GetTiles {Id}", dashboardId);
    var tiles = await _tileService.GetAllTiles(dashboardId);
    return tiles.ToList();
  }

  public async Task<TileDto> GetTileAsync(int id)
  {
    _logger?.LogInformation("[TileController] GetTile {Id}", id);
    var tile = await _tileService.GetTile(id);
    return tile;
  }

  public async Task<bool> HasTilesAsync(Guid? dashboardId)
  {
    if (!dashboardId.HasValue) { return false; }
    var hasTiles = await _tileService.HasTiles(dashboardId.Value);
    return hasTiles;
  }

  public Task<TileDto> UpdateTileAsync(int id, TileDto body)
  {
    throw new NotImplementedException();
  }
}
