using AutoMapper;
using MassTransit;
using TheDashboard.DataSourceService.BusinessLogic;
using TheDashboard.SharedEntities;

namespace TheDashboard.DataSourceService.Infrastructure.Integration;

public class DashboardAddedHandler : IConsumer<DashboardAdded>
{

  private readonly IMapper _mapper;
  private readonly IDashboardService _dashboardService;

  public DashboardAddedHandler(IMapper mapper, IDashboardService dashboardService)
  {
    _mapper = mapper;
    _dashboardService = dashboardService;
  }


  public async Task Consume(ConsumeContext<DashboardAdded> context)
  {
    var id = context.Message.Id;
    var dashboard = await _dashboardService.GetDashboard(id);
    if (dashboard == null)
    {
      dashboard = new() { Id = id, Name = context.Message.Item.Name };
      await _dashboardService.AddDashboard(dashboard);
    }
  }
}
