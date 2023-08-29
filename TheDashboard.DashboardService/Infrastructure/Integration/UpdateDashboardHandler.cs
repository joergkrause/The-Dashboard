using AutoMapper;
using MassTransit;
using TheDashboard.DashboardService.BusinessLogic;
using TheDashboard.SharedEntities;

namespace TheDashboard.DashboardService.Infrastructure.Integration;

public class UpdateDashboardHandler : IConsumer<DashboardUpdated>
{

  private readonly IMapper _mapper;
  private readonly IDashboardService _dashboardService;

  public UpdateDashboardHandler(IMapper mapper, IDashboardService dashboardService)
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
      dashboard.Name = context.Message.Item.Name;
      await _dashboardService.UpdateDashboard(dashboard);
    }
  }
}
