using TheDashboard.SharedEntities;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.Frontend.Services
{
  public interface ITileDataService
  {
    bool IsConnected { get; }

    event OnMessageEvent Message;

    Task<TileViewModel> GetTile(int id);
    Task<IList<TileViewModel>> GetTiles(Guid dashboardId);
    Task Init();
    Task InvokeCommand<TEvent>(TileViewModel dto) where TEvent : Command;
  }
}