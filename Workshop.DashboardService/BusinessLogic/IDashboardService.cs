using Workshop.Domain;
using Workshop.Services.TransferObjects;

namespace Workshop.Services
{
  public interface IDashboardService
  {
    void AddDashboard(DashboardDto dashboard);
    void DeleteDashboard(Guid id);
    DashboardDto GetDashboard(Guid id);
    IEnumerable<DashboardDto> GetDashboards();
    void UpdateDashboard(DashboardDto dashboard);
  }
}