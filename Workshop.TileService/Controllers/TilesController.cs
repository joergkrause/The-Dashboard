using Microsoft.AspNetCore.Mvc;
using Workshop.Services;
using Workshop.Services.TransferObjects;
using Workshop.TileService.BusinessLogic;

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
    private readonly ITileService _tileService;

    public TilesController(IServiceProvider serviceProvider)
    {
      _logger = serviceProvider.GetService<ILogger<TilesController>>();
      _tileService = serviceProvider.GetRequiredService<ITileService>();
    }


    // GET: api/<TilesController>
    [HttpGet("tiles/{id:int}", Name = "GetDashboardTiles")]
    [ProducesResponseType(typeof(IEnumerable<TileDto>), StatusCodes.Status200OK)]
    public IEnumerable<TileDto> GetTiles(int id)
    {
      return _tileService.GetAllTiles(id);
    }

    // GET api/<TilesController>/5
    [HttpGet("{id}", Name = "GetTile")]
    [ProducesResponseType(typeof(TileDto), StatusCodes.Status200OK)]
    public TileDto GetTile(int id)
    {
      return _tileService.GetTile(id);
    }

    // POST api/<TilesController>
    [HttpPost]
    public void Post([FromBody] TileDto value)
    {
    }

    [HttpPost("hastiles", Name = "HasTiles")]
    public bool[] HasTiles([FromBody] Guid[] value)
    {
      throw new NotImplementedException();
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
