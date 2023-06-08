using AutoMapper;
using MassTransit;
using MassTransit.Monitoring.Performance;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Workshop.DashboardService.Infrastructure.Integration.Events;
using Workshop.DatabaseLayer;
using Workshop.Domain;
using Workshop.Services.TransferObjects;

namespace Workshop.Services;

public class DashboardService : UnitOfWork, IDashboardService
{

  private readonly List<Dashboard> dashboards;
  private readonly IMapper _mapper;
  private readonly IPublishEndpoint _publishEndpoint;

  public DashboardService(DbContext context, IMapper mapper, IPublishEndpoint publishEndpoint) : base(context)
  {
    _mapper = mapper;
    _publishEndpoint = publishEndpoint;
    dashboards = new List<Dashboard>
    {
      new Dashboard()
      {
        Id = 1,
        Name = "Dashboard 1",
      }
    };
  }

  public IEnumerable<DashboardDto> GetDashboards()
  {
    var models = this.dashboards.ToList();    
    var dtos = _mapper.Map<IEnumerable<DashboardDto>>(this.dashboards);
    return dtos;
  }

  public DashboardDto GetDashboard(int id)
  {
    var model = dashboards.Single(d => d.Id == id);
    return _mapper.Map<DashboardDto>(model);
  }

  public void AddDashboard(DashboardDto dto)
  {
    var dashboard = _mapper.Map<Dashboard>(dto);
    var createdEvent = new DashboardCreatedEvent(dashboard.Id, dashboard.Name);
    _publishEndpoint.Publish(createdEvent);
    dashboards.Add(dashboard);
  }

  public void UpdateDashboard(DashboardDto dto)
  {
    var dashboard = _mapper.Map<Dashboard>(dto);
    DeleteDashboard(dashboard.Id);
    dashboards.Add(dashboard);

  }

  public async Task DeleteDashboard(int id)
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
