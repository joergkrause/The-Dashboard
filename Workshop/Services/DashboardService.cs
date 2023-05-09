using AutoMapper;
using Workshop.Domain;
using Workshop.Services.TransferObjects;

namespace Workshop.Services;

public class DashboardService : IDashboardService
{

  private readonly List<Dashboard> dashboards;
  private readonly IMapper _mapper;

  public DashboardService(IMapper mapper)
  {
    _mapper = mapper;
    dashboards = new List<Dashboard>
    {
      new Dashboard()
      {
        Id = 1,
        Name = "Dashboard 1",
        Tiles = new[]
      {
        new Tile () { Id = 1, Url = "https://www.google.com", Title = "Google" },
        new Tile () { Id = 2, Url = "https://www.microsoft.com", Title = "Microsoft" },
      }
      }
    };
  }

  public IEnumerable<DashboardDto> GetDashboards()
  {
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
    dashboards.Add(dashboard);
  }

  public void UpdateDashboard(DashboardDto dto)
  {
    var dashboard = _mapper.Map<Dashboard>(dto);
    DeleteDashboard(dashboard.Id);
    dashboards.Add(dashboard);
  }

  public void DeleteDashboard(int id)
  {
    var dashboard = dashboards.Single(d => d.Id == id);
    dashboards.Remove(dashboard);
  }

}
