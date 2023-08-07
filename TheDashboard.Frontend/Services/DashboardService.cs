using AutoMapper;
using System.Text;
using System.Text.Json;
using TheDashboard.SharedEntities;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.Frontend.Services;

public sealed class DashboardService : ServiceInvokeCommand, IDashboardService
{
  
  private readonly IDashboardClient _dashboardClient;
  private readonly ITilesClient _tilesClient;

  public DashboardService(IMapper mapper, IHttpClientFactory httpClientFactory, IDashboardClient dashboardClient, ITilesClient tilesClient) : base(mapper, httpClientFactory)
  {
    
    _dashboardClient = dashboardClient;
    _tilesClient = tilesClient;
  }

  public async Task<IList<DashboardViewModel>> GetDashboards()
  {
    var dashboards = await _dashboardClient.GetAllAsync();
    var viewModels = Mapper.Map<IList<DashboardViewModel>>(dashboards);
    return viewModels;
  }

  public async Task<DashboardViewModel> GetDashboard(Guid id)
  {
    var dashboard = await _dashboardClient.GetAsync(id);
    var viewModel = Mapper.Map<DashboardViewModel>(dashboard);
    return viewModel;
  }

  public async Task<IList<TileViewModel>> GetAssignedTiles(Guid dashboardId)
  {
    var tiles = await _tilesClient.GetDashboardTilesAsync(dashboardId);
    return Mapper.Map<List<TileViewModel>>(tiles);
  }

  public async Task InvokeCommand<TEvent>(DashboardViewModel model) where TEvent : Command
  {
    await base.InvokeCommand<TEvent, DashboardViewModel, Guid>(model);
  }
}
