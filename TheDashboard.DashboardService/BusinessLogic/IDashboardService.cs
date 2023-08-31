using System.Linq.Expressions;
using TheDashboard.DashboardService.Domain;
using TheDashboard.SharedEntities;

namespace TheDashboard.DashboardService.BusinessLogic
{
  public interface IDashboardService
  {
    Task<DashboardDto> AddDashboard(DashboardDto dashboard);
    Task DeleteDashboard(Guid id);
    Task<DashboardDto> GetDashboard(Guid id);
    Task<bool> DashboardExists(Guid id);
    Task<IEnumerable<DashboardDto>> GetDashboards(params Expression<Func<Dashboard, object>>[] includes);
    Task UpdateDashboard(DashboardDto dashboard);
  }
}