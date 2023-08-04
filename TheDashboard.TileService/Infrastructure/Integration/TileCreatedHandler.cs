using AutoMapper;
using MassTransit;
using TheDashboard.SharedEntities;
using TheDashboard.TileService.BusinessLogic;

namespace TheDashboard.TileService.Infrastructure.Integration;

public class TileCreatedHandler : IConsumer<TileAdded>
{

  private readonly IMapper _mapper;
  private readonly ITileService _tileService;

  public TileCreatedHandler(IMapper mapper, ITileService tileService)
  {
    _mapper = mapper;
    _tileService = tileService;
  }


  public async Task Consume(ConsumeContext<TileAdded> context)
  {
    var tileDto = _mapper.Map<TileDto>(context.Message.Item);
    await _tileService.AddTile(tileDto);
  }
}
