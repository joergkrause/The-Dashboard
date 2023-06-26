using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TheDashboard.DashboardService.Domain;
using TheDashboard.Services;
using TheDashboard.Services.TransferObjects;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheDashboard.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class LayoutController : ControllerBase
{

  private readonly ILogger<DashboardController>? _logger;
  private readonly ILayoutService _layoutService;

  public LayoutController(IServiceProvider serviceProvider)
  {
    _logger = serviceProvider.GetService<ILogger<DashboardController>>();
    _layoutService = serviceProvider.GetRequiredService<ILayoutService>();

  }

  [HttpGet(Name = "GetLayouts", Order = 10)]
  [ProducesResponseType(typeof(IEnumerable<LayoutDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> Get()
  {
    _logger?.LogInformation("GetUser called");
    var userModels = await _layoutService.GetUserLayouts();
    var adminModels = await _layoutService.GetAdminLayouts();
    return Ok(userModels.Union(adminModels));
  }

  [HttpGet("user", Name = "GetUserLayouts", Order = 20)]
  [ProducesResponseType(typeof(IEnumerable<LayoutDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetUser()
  {
    _logger?.LogInformation("GetUser called");
    var models = await _layoutService.GetUserLayouts();
    return Ok(models);
  }

  [HttpGet("admin", Name = "GetAdminLayouts", Order = 30)]
  [ProducesResponseType(typeof(IEnumerable<LayoutDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAdmin()
  {
    _logger?.LogInformation("GetAdmin called");
    var models = await _layoutService.GetAdminLayouts();
    return Ok(models);
  }

  [HttpGet("{id:int}", Name = "GetLayout")] // 400 Bad Request
  [ProducesResponseType(typeof(LayoutDto), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> GetById(int id)
  {
    var model = await _layoutService.Get(id);
    return Ok(model);
  }

  [HttpPost(Name = "AddLayout")]
  [ProducesResponseType(typeof(LayoutDto), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Post([FromBody] LayoutDto value)
  {
    if (ModelState.IsValid)
    {
      await _layoutService.AddUserLayout(value);
      return CreatedAtAction(nameof(Get), new { id = value.Id }, value);
    }
    else
    {
      return BadRequest(ModelState);
    }
  }

  [HttpPut("{id:int}", Name = "UpdateLayout")]
  [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(void), StatusCodes.Status202Accepted)]
  [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Put(int id, [FromBody] LayoutDto dto)
  {
    if (id == 0)
    {
      return NotFound(); // 404
    }
    if (ModelState.IsValid)
    {
      await _layoutService.UpdateLayout(dto);
      return Accepted();  // 200/201/202/204 
    }
    else
    {
      return BadRequest(); // 400 Bad Request "nö"
    }
  }

  [HttpDelete("{id:int}", Name = "RemoveLayout")]
  [ProducesResponseType(typeof(void), StatusCodes.Status202Accepted)]
  public async Task<IActionResult> Delete(int id)
  {
    await _layoutService.RemoveLayout(id);
    return Accepted();
  }
}
