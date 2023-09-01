using AutoMapper;
using MassTransit;
using TheDashboard.DashboardService.BusinessLogic;
using TheDashboard.SharedEntities;

namespace TheDashboard.DashboardService.Infrastructure.Integration;

public class AddDashboardHandler : IConsumer<AddDashboard>
{

  private readonly IDashboardService _dashboardService;

  public AddDashboardHandler(IDashboardService dashboardService)
  {
    _dashboardService = dashboardService;
  }


  public async Task Consume(ConsumeContext<AddDashboard> context)
  {
    var id = context.Message.Id;
    var exists = await _dashboardService.DashboardExists(id);
    if (!exists)
    {
      await _dashboardService.AddDashboard(context.Message.Item);      
    }
  }
}
