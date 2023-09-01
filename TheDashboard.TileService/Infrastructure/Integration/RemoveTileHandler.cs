using AutoMapper;
using MassTransit;
using TheDashboard.SharedEntities;
using TheDashboard.TileService.BusinessLogic;

namespace TheDashboard.TileService.Infrastructure.Integration;

public class RemoveTileHandler : IConsumer<RemoveTile>
{

  private readonly IMapper _mapper;
  private readonly ITileService _tileService;

  public RemoveTileHandler(IMapper mapper, ITileService tileService)
  {
    _mapper = mapper;
    _tileService = tileService;
  }


  public async Task Consume(ConsumeContext<RemoveTile> context)
  {
    var id = context.Message.TileId;
    await _tileService.DeleteTile(id);
  }
}
