﻿using AutoMapper;
using MassTransit;
using TheDashboard.BuildingBlocks.Core.EventStore;
using TheDashboard.TileService.BusinessLogic;
using TheDashboard.TileService.Controllers.Models;

namespace TheDashboard.TileService.Infrastructure.Integration;

public class TileUpdatedHandler : IConsumer<TileUpdated>
{

  private readonly IMapper _mapper;
  private readonly ITileService _tileService;

  public TileUpdatedHandler(IMapper mapper, ITileService tileService)
  {
    _mapper = mapper;
    _tileService = tileService;
  }


  public async Task Consume(ConsumeContext<TileUpdated> context)
  {
    var tileDto = _mapper.Map<TileDto>(context.Message.Item);
    await _tileService.UpdateTile(tileDto);
  }
}
