using TheDashboard.BuildingBlocks.Core.EventStore;
using TheDashboard.Clients;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.FrontendUi.Services
{
  public interface IDashboardService
  {

    Task InvokeCommand<TEvent>(DashboardDto dto) where TEvent : Command;

    Task<IList<TileViewModel>> GetTiles(Guid dashboardId);

        
    }
}