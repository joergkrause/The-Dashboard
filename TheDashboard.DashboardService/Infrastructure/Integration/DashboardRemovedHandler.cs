using AutoMapper;
using MassTransit;
using TheDashboard.Services;
using TheDashboard.SharedEntities;

namespace TheDashboard.DashboardService.Infrastructure.Integration;

public class DashboardRemovedHandler : IConsumer<DashboardRemoved>
{

  private readonly IMapper _mapper;
  private readonly IDashboardService _dashboardService;

  public DashboardRemovedHandler(IMapper mapper, IDashboardService dashboardService)
  {
    _mapper = mapper;
    _dashboardService = dashboardService;
  }


  public async Task Consume(ConsumeContext<DashboardRemoved> context)
  {
    var id = context.Message.Id;    
    var dashboard = await _dashboardService.GetDashboard(id);
    if (dashboard != null)
    {
      await _dashboardService.DeleteDashboard(id);
    }
  }
}
