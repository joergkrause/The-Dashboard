using AutoMapper;
using MassTransit;
using TheDashboard.TileService.BusinessLogic;

namespace TheDashboard.TileService.Infrastructure.Integration;

public class TileRemovedHandler : IConsumer<TileRemovedEvent>
{

  private readonly IMapper _mapper;
  private readonly ITileService _tileService;

  public TileRemovedHandler(IMapper mapper, ITileService tileService)
  {
    _mapper = mapper;
    _tileService = tileService;
  }


  public async Task Consume(ConsumeContext<TileRemovedEvent> context)
  {
    var id = context.Message.Id;
    await _tileService.DeleteTile(id);
  }
}
