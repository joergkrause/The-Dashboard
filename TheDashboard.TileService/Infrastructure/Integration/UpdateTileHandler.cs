using AutoMapper;
using MassTransit;
using TheDashboard.SharedEntities;
using TheDashboard.TileService.BusinessLogic;

namespace TheDashboard.TileService.Infrastructure.Integration;

public class UpdateTileHandler : IConsumer<UpdateTile>
{

  private readonly IMapper _mapper;
  private readonly ITileService _tileService;

  public UpdateTileHandler(IMapper mapper, ITileService tileService)
  {
    _mapper = mapper;
    _tileService = tileService;
  }


  public async Task Consume(ConsumeContext<UpdateTile> context)
  {
    var tileDto = _mapper.Map<TileDto>(context.Message.Item);
    await _tileService.UpdateTile(tileDto);
  }
}
