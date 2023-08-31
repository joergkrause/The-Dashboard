using AutoMapper;
using MassTransit;
using TheDashboard.DashboardService.BusinessLogic;
using TheDashboard.SharedEntities;

namespace TheDashboard.DashboardService.Infrastructure.Integration;

public class AddDashboardHandler : IConsumer<AddDashboard>
{

  private readonly IMapper _mapper;
  private readonly IDashboardService _dashboardService;

  public AddDashboardHandler(IMapper mapper, IDashboardService dashboardService)
  {
    _mapper = mapper;
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
