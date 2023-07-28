using AutoMapper;
using MassTransit;
using TheDashboard.TileService.BusinessLogic;
using TheDashboard.TileService.Controllers.Models;

namespace TheDashboard.TileService.Infrastructure.Integration;

public class TileCreatedHandler : IConsumer<TileCreatedEvent>
{

  private readonly IMapper _mapper;
  private readonly ITileService _tileService;

  public TileCreatedHandler(IMapper mapper, ITileService tileService)
  {
    _mapper = mapper;
    _tileService = tileService;
  }


  public async Task Consume(ConsumeContext<TileCreatedEvent> context)
  {
    var tileDto = _mapper.Map<TileDto>(context.Message.TileDto);
    await _tileService.AddTile(tileDto);
  }
}
