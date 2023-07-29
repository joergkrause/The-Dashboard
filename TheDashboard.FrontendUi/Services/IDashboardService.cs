using TheDashboard.ViewModels.Data;

namespace TheDashboard.FrontendUi.Services
{
  public interface IDashboardService
  {
    Task<IList<TileViewModel>> GetTiles(Guid dashboardId);
  }
}