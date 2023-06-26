using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheDashboard.DataConsumerService.Domain;
using TheDashboard.DataConsumerService.Infrastructure;
using TheDashboard.DataConsumerService.TransferObjects;

namespace TheDashboard.DataConsumerService.BusinessLogic;

public class DashboardService : IDashboardService
{

  private readonly ILogger<DashboardService> _logger;
  private readonly DataConsumerDbContext _dataconsumerDbContext;
  private readonly IMapper _mapper;

  public DashboardService(ILogger<DashboardService> logger, DataConsumerDbContext tileDbContext, IMapper mapper)
  {
    _logger = logger;
    _dataconsumerDbContext = tileDbContext;
    _mapper = mapper;
  }

  public async Task<DashboardDto?> GetDashboard(Guid dashboardId)
  {
    var model = await _dataconsumerDbContext.Set<DashboardDto>().SingleOrDefaultAsync(e => e.Id == dashboardId);
    if (model == null)
    {
      return null;
    }
    return _mapper.Map<DashboardDto>(model);
  }

  public async Task<IEnumerable<DashboardDto>> GetAllDashboards()
  {
    var model = await _dataconsumerDbContext.Set<Dashboard>().ToListAsync();
    return _mapper.Map<IEnumerable<DashboardDto>>(model);
  }

  public async Task<DashboardDto> AddDashboard(DashboardDto dashboardDto)
  {
    var model = _mapper.Map<Dashboard>(dashboardDto);
    _dataconsumerDbContext.Set<Dashboard>().Add(model);
    await _dataconsumerDbContext.SaveChangesAsync();
    return _mapper.Map<DashboardDto>(model);
  }

  public async Task<DashboardDto> UpdateDashboard(DashboardDto dashboardDto)
  {
    var model = await _dataconsumerDbContext.Set<Dashboard>().SingleOrDefaultAsync(e => e.Id == dashboardDto.Id);
    if (model == null)
    {
      return null!;
    }
    model.Name = dashboardDto.Name;
    await _dataconsumerDbContext.SaveChangesAsync();
    return _mapper.Map<DashboardDto>(model);
  }

  public async Task<bool> HasDashboards()
  {
    return await _dataconsumerDbContext.Set<Dashboard>().AnyAsync();
  }

  public async Task<bool> HasDashboard(Guid dashboardId)
  {
    return await _dataconsumerDbContext.Set<DashboardDto>().AnyAsync(e => e.Id == dashboardId);
  }

  public async Task DeleteDashboard(Guid dashboardId)
  {
    var model = await _dataconsumerDbContext.Set<Dashboard>().SingleOrDefaultAsync(e => e.Id == dashboardId);
    if (model == null)
    {
      return;
    }
    _dataconsumerDbContext.Set<Dashboard>().Remove(model);
    await _dataconsumerDbContext.SaveChangesAsync();
  }

}
