using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proxy;

namespace FrontendApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DashboardController : ControllerBase
  {
    private readonly DashboardProxy _proxyService;

    public DashboardController(DashboardProxy dashboardProxy)
    {
      _proxyService = dashboardProxy;
    }

    public async Task<IEnumerable<Dashboard>> GetDashboards()
    {
      var dashboards = await _proxyService.DashboardAllAsync();
      return dashboards;
    }

    public async Task<IEnumerable<Tile>> GetTiles(int dashboardId)
    {
      var dashboards = await _proxyService.DashboardAllAsync();
      return null;
    }

  }
}
