using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheDashboard.SharedEntities;
using TheDashboard.TileService.Domain;
using TheDashboard.TileService.Infrastructure;

namespace TheDashboard.TileService.BusinessLogic;

public class DashboardService : IDashboardService
{

  private readonly ILogger<DashboardService> _logger;
  private readonly TileDbContext _tileDbContext;
  private readonly IMapper _mapper;

  public DashboardService(ILogger<DashboardService> logger, TileDbContext tileDbContext, IMapper mapper)
  {
    _logger = logger;
    _tileDbContext = tileDbContext;
    _mapper = mapper;
  }

  public async Task<DashboardDto?> GetDashboard(Guid dashboardId)
  {
    var model = await _tileDbContext.Set<Dashboard>().SingleOrDefaultAsync(e => e.Id == dashboardId);
    if (model == null)
    {
      return null;
    }
    return _mapper.Map<DashboardDto>(model);
  }

  public async Task<IEnumerable<DashboardDto>> GetAllDashboards()
  {
    var model = await _tileDbContext.Set<Dashboard>().ToListAsync();
    return _mapper.Map<IEnumerable<DashboardDto>>(model);
  }

  public async Task<DashboardDto> AddDashboard(DashboardDto dashboardDto)
  {
    var model = _mapper.Map<Dashboard>(dashboardDto);
    _tileDbContext.Set<Dashboard>().Add(model);
    await _tileDbContext.SaveChangesAsync();
    return _mapper.Map<DashboardDto>(model);
  }

  public async Task<DashboardDto> UpdateDashboard(DashboardDto dashboardDto)
  {
    var model = await _tileDbContext.Set<Dashboard>().SingleOrDefaultAsync(e => e.Id == dashboardDto.Id);
    if (model == null)
    {
      return null!;
    }
    model.Name = dashboardDto.Name;
    await _tileDbContext.SaveChangesAsync();
    return _mapper.Map<DashboardDto>(model);
  }

  public async Task<bool> HasDashboards()
  {
    return await _tileDbContext.Set<Dashboard>().AnyAsync();
  }

  public async Task<bool> HasDashboard(Guid dashboardId)
  {
    return await _tileDbContext.Set<DashboardDto>().AnyAsync(e => e.Id == dashboardId);
  }

  public async Task DeleteDashboard(Guid dashboardId)
  {
    var model = await _tileDbContext.Set<Dashboard>().SingleOrDefaultAsync(e => e.Id == dashboardId);
    if (model == null)
    {
      return;
    }
    _tileDbContext.Set<Dashboard>().Remove(model);
    await _tileDbContext.SaveChangesAsync();
  }

}
