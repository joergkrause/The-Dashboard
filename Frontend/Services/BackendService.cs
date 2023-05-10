using Frontend.Proxy;

namespace Frontend.Services;

public class BackendService
{

  private readonly DashboardProxy _dashboardProxy;

  public BackendService(DashboardProxy dashboardProxy)
  {
     _dashboardProxy = dashboardProxy;
  }

  public async Task<DashboardDto> GetDashboard(int id)
  {
    return await _dashboardProxy.GetAsync(id);
  }

  public async Task<IEnumerable<TileDto>> GetTiles(int id)
  {
    return await _dashboardProxy.GetDashboardTilesAsync(id);
  }

}
