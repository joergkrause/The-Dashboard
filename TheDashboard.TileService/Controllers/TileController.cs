using Microsoft.AspNetCore.Mvc;
using TheDashboard.TileService.BusinessLogic;
using TheDashboard.TileService.Controllers.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheDashboard.TileService.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(500)]
[Produces("application/json")]
public class TileController : ControllerBase
{

  private readonly ILogger<TileController>? _logger;
  private readonly ITileService _tileService;

  public TileController(IServiceProvider serviceProvider)
  {
    _logger = serviceProvider.GetService<ILogger<TileController>>();
    _tileService = serviceProvider.GetRequiredService<ITileService>();
  }

  [HttpGet("all/{dashboardId:guid}", Name = "GetDashboardTiles")]
  [ProducesResponseType(typeof(IEnumerable<TileDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetTiles(Guid dashboardId)
  {
    _logger?.LogInformation("[TileController] GetTiles {Id}", dashboardId);
    var tiles = await _tileService.GetAllTiles(dashboardId);
    return Ok(tiles);
  }

  [HttpGet("{id:int}", Name = "GetTile")]
  [ProducesResponseType(typeof(TileDto), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetTile(int id)
  {
    var tile = await _tileService.GetTile(id);
    if (tile == null)
    {
      return NotFound();
    }
    return Ok(tile);
  }

  [HttpPost]
  [ProducesResponseType(typeof(TileDto), StatusCodes.Status201Created)]
  public async Task<IActionResult> Insert([FromBody] TileDto tileDto)
  {
    var tile = await _tileService.AddTile(tileDto);
    return CreatedAtAction(nameof(GetTile), new { id = tile.Id }, tile);
  }

  [HttpPost("hastiles", Name = "HasTiles")]
  [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
  public async Task<IActionResult> HasTiles([FromBody] Guid dashboardId)
  {
    var hasTiles = await _tileService.HasTiles(dashboardId);
    return Ok(hasTiles);
  }

  [HttpPut("{id}")]
  [ProducesResponseType(typeof(TileDto), StatusCodes.Status202Accepted)]
  public async Task<IActionResult> Update(int id, [FromBody] TileDto tileDto)
  {
    tileDto.Id = id;
    var tile = await _tileService.UpdateTile(tileDto);
    return Accepted(tile);
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(typeof(TileDto), StatusCodes.Status204NoContent)]
  public async Task<IActionResult> Delete(int id)
  {
    await _tileService.DeleteTile(id);
    return NoContent();
  }
}
