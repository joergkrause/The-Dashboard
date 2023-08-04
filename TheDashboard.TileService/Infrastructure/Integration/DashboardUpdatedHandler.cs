using AutoMapper;
using MassTransit;
using TheDashboard.SharedEntities;
using TheDashboard.TileService.BusinessLogic;

namespace TheDashboard.TileService.Infrastructure.Integration;

public class DashboardUpdatedHandler : IConsumer<DashboardUpdated>
{

  private readonly IMapper _mapper;
  private readonly IDashboardService _dashboardService;

  public DashboardUpdatedHandler(IMapper mapper, IDashboardService dashboardService)
  {
    _mapper = mapper;
    _dashboardService = dashboardService;
  }


  public async Task Consume(ConsumeContext<DashboardUpdated> context)
  {
    var id = context.Message.Id;    
    var dashboard = await _dashboardService.GetDashboard(id);
    if (dashboard != null)
    {
      dashboard = context.Message.Item;
      await _dashboardService.UpdateDashboard(dashboard);
    }
  }
}
