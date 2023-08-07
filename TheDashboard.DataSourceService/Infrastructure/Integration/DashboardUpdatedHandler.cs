using AutoMapper;
using MassTransit;
using TheDashboard.DataConsumerService.BusinessLogic;
using TheDashboard.SharedEntities;

namespace TheDashboard.DataConsumerService.Infrastructure.Integration;

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
      dashboard.Name = context.Message.Item.Name;
      await _dashboardService.UpdateDashboard(dashboard);
    }
  }
}
