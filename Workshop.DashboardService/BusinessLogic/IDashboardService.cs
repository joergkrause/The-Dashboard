using System.Linq.Expressions;
using Workshop.Domain;
using Workshop.Services.TransferObjects;

namespace Workshop.Services
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