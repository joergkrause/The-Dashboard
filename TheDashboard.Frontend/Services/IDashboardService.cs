using TheDashboard.SharedEntities;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.Frontend.Services
{
  public interface IDashboardService
  {

    Task InvokeCommand<TEvent>(DashboardDto dto) where TEvent : Command;

    Task<IList<TileViewModel>> GetTiles(Guid dashboardId);

        
    }
}