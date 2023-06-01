using Workshop.Domain;
using Workshop.Services.TransferObjects;

namespace Workshop.Services
{
  public interface IDashboardService
  {
    Task AddDashboard(DashboardDto dashboard);
    Task DeleteDashboard(Guid id);
    Task<DashboardDto> GetDashboard(Guid id);
    Task<IEnumerable<DashboardDto>> GetDashboards();
    Task UpdateDashboard(DashboardDto dashboard);
  }
}