﻿using TheDashboard.TileService.Controllers.Models;

namespace TheDashboard.TileService.BusinessLogic
{
  public interface IDashboardService
  {
    Task<DashboardDto> AddDashboard(DashboardDto dashboardDto);
    Task DeleteDashboard(Guid dashboardId);
    Task<IEnumerable<DashboardDto>> GetAllDashboards();
    Task<DashboardDto?> GetDashboard(Guid dashboardId);
    Task<bool> HasDashboard(Guid dashboardId);
    Task<bool> HasDashboards();
    Task<DashboardDto> UpdateDashboard(DashboardDto dashboardDto);
  }
}