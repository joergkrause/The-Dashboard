using Microsoft.AspNetCore.Mvc;
using Workshop.Services.TransferObjects;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Workshop.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TilesController : ControllerBase
  {
    // GET: api/<TilesController>
    [HttpGet]
    public IEnumerable<TileDto> Get()
    {
      return null;
    }

    // GET api/<TilesController>/5
    [HttpGet("{id}")]
    public TileDto Get(int id)
    {
      return null;
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
