using AutoMapper;
using MassTransit;
using MassTransit.Monitoring.Performance;
using MassTransit.Transports;
using Workshop.DashboardService.Infrastructure.Integration.Events;
using Workshop.Domain;
using Workshop.Services.TransferObjects;

namespace Workshop.Services;

public class DashboardService : IDashboardService
{

  private readonly List<Dashboard> dashboards;
  private readonly IMapper _mapper;
  private readonly IPublishEndpoint _publishEndpoint;

  public DashboardService(IMapper mapper, IPublishEndpoint publishEndpoint)
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

  public IEnumerable<DashboardDto> GetDashboards()
  {
    var dtos = _mapper.Map<IEnumerable<DashboardDto>>(this.dashboards);
    return dtos;
  }

  public DashboardDto GetDashboard(Guid id)
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

  public void DeleteDashboard(Guid id)
  {
    var dashboard = dashboards.Single(d => d.Id == id);
    dashboards.Remove(dashboard);
  }

}
