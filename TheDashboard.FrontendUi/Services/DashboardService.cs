using AutoMapper;
using System.Text;
using System.Text.Json;
using TheDashboard.BuildingBlocks.Core.EventStore;
using TheDashboard.Clients;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.FrontendUi.Services;

public class DashboardService : IDashboardService
{
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private readonly IDashboardClient _dashboardClient;
    private readonly ITilesClient _tilesClient;
    private readonly IDataConsumerClient _dataConsumerClient;

    public DashboardService(IMapper mapper, IHttpClientFactory httpClientFactory, IDashboardClient dashboardClient, ITilesClient tilesClient, IDataConsumerClient dataConsumerClient)
    {
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient("HttpCommandProxy");
        _dashboardClient = dashboardClient;
        _tilesClient = tilesClient;
        _dataConsumerClient = dataConsumerClient;
    }

    public async Task<IList<TileViewModel>> GetTiles(Guid dashboardId)
    {
        var tiles = await _tilesClient.GetDashboardTilesAsync(dashboardId);
        return _mapper.Map<List<TileViewModel>>(tiles);
    }

    public async Task InvokeCommand<TEvent>(DashboardDto dto) where TEvent : Command
    {
        var evt = _mapper.Map<TEvent>(dto);
        await _httpClient.SendAsync(request: new HttpRequestMessage(HttpMethod.Post, "/api/command")
        {
            Content = new StringContent(JsonSerializer.Serialize(evt), Encoding.UTF8, "application/json")
        });
    }
}
