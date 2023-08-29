using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TheDashboard.DashboardService.Infrastructure;
using TheDashboard.DatabaseLayer;
using TheDashboard.DashboardService.Domain;
using TheDashboard.SharedEntities;

namespace TheDashboard.DashboardService.BusinessLogic;

public class DashboardService : UnitOfWork<DashboardContext>, IDashboardService
{
  private readonly ILogger<DashboardService> _logger;
  private readonly IMapper _mapper;
  private readonly IPublishEndpoint? _publishEndpoint;

  public DashboardService(ILogger<DashboardService> logger, DashboardContext context, IServiceProvider serviceProvider) : base(context)
  {
    _logger = logger;
    _mapper = serviceProvider.GetRequiredService<IMapper>();
    _publishEndpoint = serviceProvider.GetService<IPublishEndpoint>();
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

  /// <summary>
  /// Add a dashboard to local service's database and publish as "Added" event to all other services.
  /// </summary>
  /// <param name="dto"></param>
  /// <returns></returns>
  public async Task<DashboardDto> AddDashboard(DashboardDto dto)
  {
    var dashboard = _mapper.Map<Dashboard>(dto);
    // add some defaults
    var defaultLayout = await Context.Layouts.SingleAsync(l => l.Id == dto.LayoutId);
    dashboard.Layout = defaultLayout;
    dashboard.Theme = "Light";
    var createdEvent = new DashboardAdded(dashboard.Id, dto);
    await (_publishEndpoint?.Publish(createdEvent) ?? Task.CompletedTask);
    Context.Dashboards.Add(dashboard);
    Context.Entry(defaultLayout).State = EntityState.Unchanged;
    try
    {
      await Context.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
      _logger.LogError("Exception adding Dashboard: {Message}", ex.Message);
      throw; // TODO: enapsulate exceptions
    }
    var dashboardDto = _mapper.Map<DashboardDto>(dashboard);
    return dashboardDto;    
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

  public void CleanUp()
  {
    BeginTransaction();

    // Remove all dashboards of a tenant

    Commit();
  }

}
