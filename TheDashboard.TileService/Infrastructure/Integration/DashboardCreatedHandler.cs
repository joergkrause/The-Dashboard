using AutoMapper;
using MassTransit;
using TheDashboard.TileService.BusinessLogic;

namespace TheDashboard.TileService.Infrastructure.Integration;

public class DashboardCreatedHandler : IConsumer<DashboardCreatedEvent>
{

  private readonly IMapper _mapper;
  private readonly IDashboardService _dashboardService;

  public DashboardCreatedHandler(IMapper mapper, IDashboardService dashboardService)
  {
    _mapper = mapper;
    _dashboardService = dashboardService;
  }


  public async Task Consume(ConsumeContext<DashboardCreatedEvent> context)
  {
    var id = context.Message.Id;    
    var dashboard = await _dashboardService.GetDashboard(id);
    if (dashboard == null)
    {
      dashboard = new() { Id = id, Name = context.Message.Name };
      await _dashboardService.AddDashboard(dashboard);
    }
  }
}
