using AutoMapper;
using MassTransit;
using TheDashboard.DashboardService.BusinessLogic;
using TheDashboard.SharedEntities;

namespace TheDashboard.DashboardService.Infrastructure.Integration;

public class RemoveDashboardHandler : IConsumer<RemoveDashboard>
{

  private readonly IMapper _mapper;
  private readonly IDashboardService _dashboardService;

  public RemoveDashboardHandler(IMapper mapper, IDashboardService dashboardService)
  {
    _mapper = mapper;
    _dashboardService = dashboardService;
  }


  public async Task Consume(ConsumeContext<RemoveDashboard> context)
  {
    var id = context.Message.Id;
    var dashboard = await _dashboardService.GetDashboard(id);
    if (dashboard != null)
    {
      await _dashboardService.DeleteDashboard(id);
    }
  }
}
