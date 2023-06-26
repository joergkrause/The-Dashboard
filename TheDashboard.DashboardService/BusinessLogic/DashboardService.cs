using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TheDashboard.DashboardService.Infrastructure;
using TheDashboard.DashboardService.Infrastructure.Integration.Events;
using TheDashboard.DatabaseLayer;
using TheDashboard.Services.TransferObjects;
using TheDashboard.DashboardService.Domain;

namespace TheDashboard.Services;

public class DashboardService : UnitOfWork<DashboardContext>, IDashboardService
{

  private readonly IMapper _mapper;
  private readonly IPublishEndpoint _publishEndpoint;

  public DashboardService(DashboardContext context, IMapper mapper, IPublishEndpoint publishEndpoint) : base(context)
  {
    _mapper = mapper;
    _publishEndpoint = publishEndpoint;
  }

  public async Task<IEnumerable<DashboardDto>> GetDashboards(params Expression<Func<Dashboard, object>>[] includes)
  {

    IQueryable<Dashboard> query = Context.Set<Dashboard>();
    foreach (var include in includes)
    {
      query = query.Include(include);
    }

    var dtos = _mapper.Map<IEnumerable<DashboardDto>>(query);
    return await Task.FromResult(dtos);
  }

  public async Task<IEnumerable<DashboardDto>> GetDashboardWithLayout(bool isActive = true)
  {
    var model = await Context.Dashboards
      .Include(d => d.Layout)
      .Include(d => d.Settings)
      .ToListAsync();
    return _mapper.Map<IEnumerable<DashboardDto>>(model);
  }

  public async Task<DashboardDto> GetDashboard(Guid id)
  {
    var model = await Context.Dashboards.SingleAsync(d => d.Id == id);
    return _mapper.Map<DashboardDto>(model);
  }

  public async Task AddDashboard(DashboardDto dto)
  {
    var dashboard = _mapper.Map<Dashboard>(dto);
    var createdEvent = new DashboardCreatedEvent(dashboard.Id, dashboard.Name);
    await _publishEndpoint.Publish(createdEvent);
    Context.Dashboards.Add(dashboard);
    await Context.SaveChangesAsync();
  }

  public async Task UpdateDashboard(DashboardDto dto)
  {
    try
    {
      var dashboard = _mapper.Map<Dashboard>(dto);
      Context.Entry(dashboard).State = EntityState.Modified;
      await Context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      // TODO: Handle this
      throw;
    }

  }

  public async Task DeleteDashboard(Guid id)
  {
    var dashboard = new Dashboard { Id = id };
    Context.Entry(dashboard).State = EntityState.Deleted;

    await Context.SaveChangesAsync();
  }

  public void CleanUp() { 
    BeginTransaction();
    
    // Remove all dashboards of a tenant

    Commit();
  }

}
