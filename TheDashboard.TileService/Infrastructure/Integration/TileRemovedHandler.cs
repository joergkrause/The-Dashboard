using AutoMapper;
using MassTransit;
using TheDashboard.BuildingBlocks.Core.EventStore;
using TheDashboard.TileService.BusinessLogic;

namespace TheDashboard.TileService.Infrastructure.Integration;

public class TileRemovedHandler : IConsumer<TileRemoved>
{

  private readonly IMapper _mapper;
  private readonly ITileService _tileService;

  public TileRemovedHandler(IMapper mapper, ITileService tileService)
  {
    _mapper = mapper;
    _tileService = tileService;
  }


  public async Task Consume(ConsumeContext<TileRemoved> context)
  {
    var id = context.Message.TileId;
    await _tileService.DeleteTile(id);
  }
}
