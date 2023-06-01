using AutoMapper;
using MassTransit;
using Workshop.TileService.BusinessLogic;
using Workshop.TileService.Infrastructure.Integration.Events;

namespace Workshop.TileService.Infrastructure.Integration;

public class DashboardCreatedHandler : IConsumer<DashboardCreatedEvent>
{

  private readonly IMapper _mapper;
  private readonly ITileService _tileService;

  public DashboardCreatedHandler(IMapper mapper, ITileService tileService)
  {
    _mapper = mapper;
    _tileService = tileService;
  }


  public async Task Consume(ConsumeContext<DashboardCreatedEvent> context)
  {
    var id = context.Message.Id;
    // TODO: _tileService.
  }
}
