using Workshop.Domain;
using Workshop.Services.TransferObjects;

namespace Workshop.Services
{
  public interface IDashboardService
  {
    void AddDashboard(DashboardDto dashboard);
    void DeleteDashboard(int id);
    IEnumerable<TileDto> GetAllTiles(int id);
    DashboardDto GetDashboard(int id);
    IEnumerable<DashboardDto> GetDashboards();
    TileDto GetTile(int id);
    void UpdateDashboard(DashboardDto dashboard);
  }
}