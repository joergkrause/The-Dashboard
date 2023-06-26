using System.Linq.Expressions;
using TheDashboard.DashboardService.Domain;
using TheDashboard.Services.TransferObjects;

namespace TheDashboard.Services
{
    public interface IDashboardService
  {
    Task AddDashboard(DashboardDto dashboard);
    Task DeleteDashboard(Guid id);
    Task<DashboardDto> GetDashboard(Guid id);
    Task<IEnumerable<DashboardDto>> GetDashboards(params Expression<Func<Dashboard, object>>[] includes);
    Task UpdateDashboard(DashboardDto dashboard);
  }
}