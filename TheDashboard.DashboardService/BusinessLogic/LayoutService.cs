using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using MassTransit.Monitoring.Performance;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using TheDashboard.DashboardService.Domain;
using TheDashboard.DashboardService.Infrastructure;
using TheDashboard.DashboardService.Infrastructure.Integration.Events;
using TheDashboard.DatabaseLayer;
using TheDashboard.Services.TransferObjects;

namespace TheDashboard.Services;

public class LayoutService : UnitOfWork<DashboardContext>, ILayoutService
{

  private readonly IMapper _mapper;

  public LayoutService(DashboardContext context, IMapper mapper) : base(context)
  {
    _mapper = mapper;
  }

  public async Task<LayoutDto> Get(int id)
  {
    var layout = await Context.Layouts.SingleAsync(e => e.Id == id);    
    return _mapper.Map<LayoutDto>(layout);
  }

  public async Task<IEnumerable<LayoutDto>> GetAdminLayouts()
  {
    var adminLayouts = await Context.Layouts.OfType<AdminLayout>().ToListAsync();
    return _mapper.Map<IEnumerable<LayoutDto>>(adminLayouts);
  }

  public async Task<IEnumerable<LayoutDto>> GetUserLayouts()
  {
    var userLayouts = await Context.Layouts.OfType<UserLayout>().ToListAsync();
    return _mapper.Map<IEnumerable<LayoutDto>>(userLayouts);
  }

  public async Task<bool> AddUserLayout(LayoutDto dto)
  {
    Context.Layouts.Add(_mapper.Map<UserLayout>(dto));
    return await Context.SaveChangesAsync() > 0;
  }

  public async Task<bool> AssignLayout(Guid dashboardId, int layoutId)
  {
    var layout = await Context.Layouts.FirstOrDefaultAsync();
    var dashboard = await Context.Dashboards.FirstOrDefaultAsync(d => d.Id == dashboardId);
    if (layout == null || dashboard == null)
    {
      return false;
    }
    layout.DashboardId = dashboardId;
    return await Context.SaveChangesAsync() > 0;
  }

  public async Task RemoveLayout(int id)
  {
    Context.Layouts.Remove(new UserLayout { Id = id });
    await Context.SaveChangesAsync();
  }

  public async Task UpdateLayout(LayoutDto dto)
  {
    var layout = await Context.Layouts.SingleAsync(e => e.Id == dto.Id);
    _mapper.Map(dto, layout);
    await Context.SaveChangesAsync();
  }
}
