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
  private readonly IPublishEndpoint _publishEndpoint;

  public DashboardService(ILogger<DashboardService> logger, DashboardContext context, IServiceProvider serviceProvider) : base(context)
  {
    _logger = logger;
    _mapper = serviceProvider.GetRequiredService<IMapper>();
    _publishEndpoint = serviceProvider.GetRequiredService<IPublishEndpoint>();
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

  public async Task<bool> DashboardExists(Guid id)
  {
    return await Context.Dashboards.AnyAsync(d => d.Id == id);
  }

  /// <summary>
  /// Add a dashboard to local service's database and publish as "Added" event to all other services.
  /// </summary>
  /// <param name="dto"></param>
  /// <returns></returns>
  public async Task<DashboardDto> AddDashboard(DashboardDto dto)
  {
    // TODO: Add number in name if name already exists
    do {
      var existing = await Context.Dashboards.SingleOrDefaultAsync(d => d.Name == dto.Name);
      if (existing != null)
      {
        var lastCharacter = dto.Name.Last().ToString();
        if (Int32.TryParse(lastCharacter, out int lastNumber)) {
          lastNumber++;
        }
        if (lastNumber > 0)
        {
          dto.Name += lastNumber;
        } else
        {
          dto.Name += "1";
        }
      }
    } while (await Context.Dashboards.AnyAsync(d => d.Name == dto.Name));
    var dashboard = _mapper.Map<Dashboard>(dto);
    // add some defaults
    var defaultLayout = await Context.Layouts.SingleOrDefaultAsync(l => l.Id == dto.LayoutId);
    dashboard.Layout = defaultLayout ?? new AdminLayout();
    dashboard.Theme = "Light";
    // forward to other services using outbox pattern
    var createdEvent = new DashboardAdded(dashboard.Id, dto);
    await _publishEndpoint.Publish(createdEvent);
    // write to DB
    Context.Dashboards.Add(dashboard);
    try
    {
      await Context.SaveChangesAsync();
    }    
    catch (DbUpdateException ex)
    {
      _logger.LogError("Exception adding Dashboard: {Message}", ex.Message);      
      throw; // TODO: enapsulate exceptions
    }
    catch (Exception ex)
    {
      _logger.LogError("Generic Exception adding Dashboard: {Message}", ex.Message);
      var errorEvent = new DashboardOperationError(dashboard.Id, OperationKind.Add, ex.Message);
      await _publishEndpoint.Publish(errorEvent);
      await Context.SaveChangesAsync();
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
      var updatedEvent = new DashboardUpdated(dashboard.Id, dto);
      await _publishEndpoint.Publish(updatedEvent);
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
    var removedEvent = new DashboardRemoved(dashboard.Id);
    await _publishEndpoint.Publish(removedEvent);

    await Context.SaveChangesAsync();
  }

  public void CleanUp()
  {
    BeginTransaction();

    // Remove all dashboards of a tenant

    Commit();
  }

}
