using TheDashboard.SharedEntities;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.Frontend.Services;

public interface IDashboardService
{

  Task InvokeCommand<TEvent>(DashboardViewModel model) where TEvent : Command;

  Task<IList<DashboardViewModel>> GetDashboards();

  Task<DashboardViewModel> GetDashboard(Guid id);

  Task<IList<TileViewModel>> GetAssignedTiles(Guid dashboardId);

}