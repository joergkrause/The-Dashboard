using Microsoft.AspNetCore.Mvc;
using Workshop.Services;
using Workshop.Services.TransferObjects;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Workshop.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [ProducesResponseType(500)]
  [Produces("application/json")]
  public class TilesController : ControllerBase
  {

    private readonly ILogger<TilesController>? _logger;
    private readonly IDashboardService _dashboardService;

    public TilesController(IServiceProvider serviceProvider)
    {
      _logger = serviceProvider.GetService<ILogger<TilesController>>();
      _dashboardService = serviceProvider.GetRequiredService<IDashboardService>();
    }


    // GET: api/<TilesController>
    [HttpGet("dashboard/{id:int}", Name = "GetDashboardTiles")]
    [ProducesResponseType(typeof(IEnumerable<TileDto>), StatusCodes.Status200OK)]
    public IEnumerable<TileDto> GetTiles(int id)
    {
      return _dashboardService.GetAllTiles(id);
    }

    // GET api/<TilesController>/5
    [HttpGet("{id}", Name = "GetTile")]
    [ProducesResponseType(typeof(TileDto), StatusCodes.Status200OK)]
    public TileDto GetTile(int id)
    {
      return _dashboardService.GetTile(id);
    }

    // POST api/<TilesController>
    [HttpPost]
    public void Post([FromBody] TileDto value)
    {
    }

    // PUT api/<TilesController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] TileDto value)
    {
    }

    // DELETE api/<TilesController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
