using AutoMapper;
using MassTransit;
using TheDashboard.SharedEntities;
using TheDashboard.TileService.BusinessLogic;

namespace TheDashboard.TileService.Infrastructure.Integration;

public class AddTileHandler : IConsumer<AddTile>
{

  private readonly IMapper _mapper;
  private readonly ITileService _tileService;

  public AddTileHandler(IMapper mapper, ITileService tileService)
  {
    _mapper = mapper;
    _tileService = tileService;
  }


  public async Task Consume(ConsumeContext<AddTile> context)
  {
    var tileDto = _mapper.Map<TileDto>(context.Message.Item);
    await _tileService.AddTile(tileDto);
  }
}
