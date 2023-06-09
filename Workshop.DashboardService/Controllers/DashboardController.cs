using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Workshop.DashboardService.Infrastructure;
using Workshop.Domain;
using Workshop.Services;
using Workshop.Services.TransferObjects;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Workshop.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Produces("application/json")] // Content-Type: application/json
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public class DashboardController : ControllerBase
  {

    private readonly ILogger<DashboardController>? _logger;
    private readonly IDashboardService _dashboardService;

    public DashboardController(IServiceProvider serviceProvider)
    {
      _logger = serviceProvider.GetService<ILogger<DashboardController>>();
      _dashboardService = serviceProvider.GetRequiredService<IDashboardService>();

    }

    // GET: api/<DashboardController>
    [HttpGet(Name = "GetAll")]
    [ProducesResponseType(typeof(IEnumerable<DashboardDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
      _logger?.LogInformation("Get called");
      var models = await _dashboardService.GetDashboards();
      return Ok(models);
    }

    // GET api/<DashboardController>/abc
    [HttpGet("{id:guid}", Name = "Get")] // 400 Bad Request
    [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetId(Guid id)
    {
      var model = _dashboardService.GetDashboard(id);
      return await Task.FromResult(Ok(model));
    }

    // GET api/<DashboardController>/search?name=db1&dddd
    [HttpGet("search", Name = "Search")]
    public async Task<IActionResult> SearchName([FromQuery] string name)
    {
      throw new NotImplementedException();
    }

    // POST api/<DashboardController>
    [HttpPost(Name = "AddDashboard")]
    [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] DashboardDto value)
    {
      if (ModelState.IsValid)
      {
        await _dashboardService.AddDashboard(value);
        return CreatedAtAction(nameof(Get), new { id = value.Id }, value);
      }
      else
      {
        return BadRequest(ModelState);
      }
    }

    // PUT api/<DashboardController>/5
    [HttpPut("{id:int}", Name = "UpdateDashboard")]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(int id, [FromBody] DashboardDto value)
    {
      if (id == 0)
      {
        return NotFound(); // 404
      }
      if (ModelState.IsValid)
      {
        await _dashboardService.UpdateDashboard(value);
        return Accepted();  // 200/201/202/204 
      }
      else
      {
        return BadRequest(); // 400 Bad Request "nö"
      }
    }

    // DELETE api/<DashboardController>/5
    [HttpDelete("{id:guid}", Name = "RemoveDashboard")]
    [ProducesResponseType(typeof(void), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Delete(Guid id)
    {
      await _dashboardService.DeleteDashboard(id);
      return Accepted();
    }
  }
}
