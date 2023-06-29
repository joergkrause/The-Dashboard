using TheDashboard.ViewModels.Data;

namespace TheDashboard.FrontendUi.Services
{
  public interface IDashboardViewerService
  {
    Task<IList<TileViewModel>> GetTiles(Guid dashboardId);
  }
}