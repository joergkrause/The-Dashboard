using AutoMapper;
using System.Text;
using System.Text.Json;
using TheDashboard.SharedEntities;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.Frontend.Services;

public class DashboardService : IDashboardService
{
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private readonly IDashboardClient _dashboardClient;
    private readonly ITilesClient _tilesClient;

    public DashboardService(IMapper mapper, IHttpClientFactory httpClientFactory, IDashboardClient dashboardClient, ITilesClient tilesClient)
    {
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient("HttpCommandProxy");
        _dashboardClient = dashboardClient;
        _tilesClient = tilesClient;
    }

  public async Task<IList<DashboardViewModel>> GetDashboards()
  {
    var dashboards = await _dashboardClient.GetAllAsync();
    var viewModels = _mapper.Map<IList<DashboardViewModel>>(dashboards);
    return viewModels;
  }

  public async Task<DashboardViewModel> GetDashboard(Guid id)
  {
    var dashboard = await _dashboardClient.GetAsync(id);
    var viewModel = _mapper.Map<DashboardViewModel>(dashboard);
    return viewModel;
  }

  public async Task<IList<TileViewModel>> GetTiles(Guid dashboardId)
    {
        var tiles = await _tilesClient.GetDashboardTilesAsync(dashboardId);
        return _mapper.Map<List<TileViewModel>>(tiles);
    }

    public async Task InvokeCommand<TEvent>(DashboardViewModel dto) where TEvent : Command
    {
        var evt = _mapper.Map<TEvent>(dto);
        await _httpClient.SendAsync(request: new HttpRequestMessage(HttpMethod.Post, "/api/command")
        {
            Content = new StringContent(JsonSerializer.Serialize(evt), Encoding.UTF8, "application/json")
        });
    }
}
