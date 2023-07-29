using AutoMapper;
using TheDashboard.Clients;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.FrontendUi.Services;

public class DashboardService : IDashboardService
{
  private readonly IMapper _mapper;

  private readonly IDashboardClient _dashboardClient;
  private readonly ITilesClient _tilesClient;
  private readonly IDataConsumerClient _dataConsumerClient;

  public DashboardService(IMapper mapper, IDashboardClient dashboardClient, ITilesClient tilesClient, IDataConsumerClient dataConsumerClient)
  {
    _mapper = mapper;

    _dashboardClient = dashboardClient;
    _tilesClient = tilesClient;
    _dataConsumerClient = dataConsumerClient;
  }

  public async Task<IList<TileViewModel>> GetTiles(Guid dashboardId)
  {
    var tiles = await _tilesClient.GetDashboardTilesAsync(dashboardId);
    return _mapper.Map<List<TileViewModel>>(tiles);
  }

}
