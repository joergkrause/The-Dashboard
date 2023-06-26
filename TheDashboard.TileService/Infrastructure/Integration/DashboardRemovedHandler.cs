﻿using AutoMapper;
using MassTransit;
using TheDashboard.TileService.BusinessLogic;

namespace TheDashboard.TileService.Infrastructure.Integration;

public class DashboardRemovedHandler : IConsumer<DashboardCreatedEvent>
{

  private readonly IMapper _mapper;
  private readonly IDashboardService _dashboardService;

  public DashboardRemovedHandler(IMapper mapper, IDashboardService dashboardService)
  {
    _mapper = mapper;
    _dashboardService = dashboardService;
  }


  public async Task Consume(ConsumeContext<DashboardCreatedEvent> context)
  {
    var id = context.Message.Id;    
    var dashboard = await _dashboardService.GetDashboard(id);
    if (dashboard != null)
    {
      await _dashboardService.DeleteDashboard(id);
    }
  }
}
