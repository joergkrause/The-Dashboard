using TheDashboard.DashboardService.Domain;
using TheDashboard.Services.TransferObjects;

namespace TheDashboard.Services
{
  public interface ILayoutService
  {
    Task<LayoutDto> Get(int id);
    Task<IEnumerable<LayoutDto>> GetAdminLayouts();
    Task<IEnumerable<LayoutDto>> GetUserLayouts();
    Task<bool> AddUserLayout(LayoutDto dto);
    Task<bool> AssignLayout(Guid dashboardId, int layoutId);
    Task RemoveLayout(int id);
    Task UpdateLayout(LayoutDto value);
  }
}