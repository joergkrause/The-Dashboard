using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using MassTransit.Monitoring.Performance;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Workshop.DashboardService.Infrastructure;
using Workshop.Domain;
using Workshop.DashboardService.Infrastructure.Integration.Events;
using Workshop.DatabaseLayer;
using Workshop.Services.TransferObjects;

namespace Workshop.Services;

public class DashboardService : UnitOfWork<DashboardContext>, IDashboardService
{

  private readonly List<Dashboard> dashboards;
  private readonly IMapper _mapper;
  private readonly IPublishEndpoint _publishEndpoint;

  public DashboardService(DashboardContext context, IMapper mapper, IPublishEndpoint publishEndpoint) : base(context)
  {
    _mapper = mapper;
    _publishEndpoint = publishEndpoint;
    dashboards = new List<Dashboard>
    {
      new Dashboard()
      {
        Id = Guid.NewGuid(),
        Name = "Dashboard 1",
      }
    };
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

  public async Task<IEnumerable<DashboardDto>> GetDashboardWithTiles(bool isActive = true)
  {
    var model = await Context.Dashboards
      .Include(d => d.Tiles)
      .Where(e => e.Tiles.Any(t => t.IsActive == isActive))
      .ToListAsync();
    return _mapper.Map<IEnumerable<DashboardDto>>(model);
  }

  public async Task<DashboardDto> GetDashboard(Guid id)
  {
    var model = dashboards.Single(d => d.Id == id);
    return await Task.FromResult(_mapper.Map<DashboardDto>(model));
  }

  public async Task AddDashboard(DashboardDto dto)
  {
    var dashboard = _mapper.Map<Dashboard>(dto);
    var createdEvent = new DashboardCreatedEvent(dashboard.Id, dashboard.Name);
    await _publishEndpoint.Publish(createdEvent);
    dashboards.Add(dashboard);
    await Task.CompletedTask;
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
      // TODO: Behandle das!
      throw;
    }

  }

  public async Task DeleteDashboard(Guid id)
  {
    // V1
    // Context.Set<Dashboard>().Remove(dashboards.Single(d => d.Id == id));

    // V2
    var dashboard = new Dashboard { Id = id };
    Context.Entry(dashboard).State = EntityState.Deleted;

    await Context.SaveChangesAsync();
  }

  public void CleanUp(int dashboradId) { 
    BeginTransaction();

    

    Commit();
  }

}
