using GrpcBackend;
using static GrpcBackend.Dashboard;

namespace Frontend.Services;

public class BackendServiceGrpc
{

  private readonly DashboardClient _dashboardClient;

  public BackendServiceGrpc(DashboardClient dashboardClient)
  {
     _dashboardClient = dashboardClient;
  }

  public async Task<IEnumerable<DashboardAllReply.Types.Dashboard>> GetDashboard(int id)
  {
    var request = new DashboardAllRequest();
    var response = await _dashboardClient.GetAllAsync(request);
    return response.Dashboards;
  }

}
