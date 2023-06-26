using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheDashboard.TileService.Controllers.Models;
using TheDashboard.TileService.Domain;
using TheDashboard.TileService.Infrastructure;

namespace TheDashboard.TileService.BusinessLogic;

public class TileService : ITileService
{
  private readonly ILogger<TileService> _logger;
  private readonly TileDbContext _tileDbContext;
  private readonly IMapper _mapper;

  public TileService(ILogger<TileService> logger, TileDbContext tileDbContext, IMapper mapper)
  {
    _logger = logger;
    _tileDbContext = tileDbContext;
    _mapper = mapper;
  }

  public async Task<IEnumerable<TileDto>> GetAllTiles(Guid dashboardId)
  {
    var model = await _tileDbContext.Set<Dashboard>().Include(e => e.Tiles).SingleOrDefaultAsync(e => e.Id == dashboardId);
    if (model == null)
    {
      return Array.Empty<TileDto>();
    }
    return _mapper.Map<IEnumerable<TileDto>>(model.Tiles);
  }

  public async Task<TileDto?> GetTile(int id)
  {
    var model = await _tileDbContext.Set<Tile>().SingleOrDefaultAsync(e => e.Id == id);
    if (model == null)
    {
      return null!;
    }
    return _mapper.Map<TileDto>(model);
  }

  public async Task<TileDto> AddTile(TileDto tileDto)
  {
    var model = _mapper.Map<Tile>(tileDto);
    _tileDbContext.Set<Tile>().Add(model);
    await _tileDbContext.SaveChangesAsync();
    return _mapper.Map<TileDto>(model);
  }

  public async Task<bool> HasTiles(Guid dashboardId)
  {
    return await _tileDbContext.Set<Dashboard>().AnyAsync(e => e.Id == dashboardId);
  }

  public async Task<TileDto> UpdateTile(TileDto tileDto)
  {
    var model = await _tileDbContext.Set<Tile>().SingleOrDefaultAsync(e => e.Id == tileDto.Id);
    if (model == null)
    {
      return null!;
    }
    _mapper.Map(tileDto, model);
    await _tileDbContext.SaveChangesAsync();
    return _mapper.Map<TileDto>(model);
  }

  public async Task DeleteTile(int id)
  {
    var model = await _tileDbContext.Set<Tile>().SingleOrDefaultAsync(e => e.Id == id);
    if (model == null)
    {
      return;
    }
    _tileDbContext.Set<Tile>().Remove(model);
    await _tileDbContext.SaveChangesAsync();
  }

}
