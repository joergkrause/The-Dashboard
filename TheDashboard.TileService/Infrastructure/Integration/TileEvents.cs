using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using TheDashboard.BuildingBlocks.Core.EventBus;
using TheDashboard.TileService.Controllers.Models;

namespace TheDashboard.TileService.Infrastructure.Integration;


public record TileCreatedEvent(TileDto TileDto) : IntegrationEvent;

public record TileUpdatedEvent(int Id, TileDto TileDto) : IntegrationEvent;

public record TileRemovedEvent(int Id) : IntegrationEvent;

