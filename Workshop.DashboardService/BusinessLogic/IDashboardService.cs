using Workshop.Domain;
using Workshop.Services.TransferObjects;

namespace Workshop.Services
{
  public interface IDashboardService
  {
    Task AddDashboard(DashboardDto dashboard);
    Task DeleteDashboard(int id);
    Task<DashboardDto> GetDashboard(int id);
    Task<IEnumerable<DashboardDto>> GetDashboards();
    Task UpdateDashboard(DashboardDto dashboard);
  }
}