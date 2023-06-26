using Microsoft.AspNetCore.Mvc;
using TheDashboard.DataConsumerService.BusinessLogic;
using TheDashboard.DataConsumerService.TransferObjects;

namespace TheDatabase.DataConsumerService.Controllers;


[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[Produces("application/json")]
public class ConsumerController : ControllerBase
{

  private readonly ILogger<ConsumerController>? _logger;
  private readonly IDataConsumerService _dataConsumerService;

  public ConsumerController(IServiceProvider serviceProvider)
  {
    _logger = serviceProvider.GetService<ILogger<ConsumerController>>();
    _dataConsumerService = serviceProvider.GetRequiredService<IDataConsumerService>();
  }

  [HttpGet(Name = "GetAll")]
  [ProducesResponseType(typeof(IEnumerable<DataSourceDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> Get()
  {
    try
    {
      var sources = await _dataConsumerService.GetAllDataSource();
      return Ok(sources);
    }
    catch (Exception ex)
    {
      _logger?.LogError(ex, "Error while getting all dashboards");
      return StatusCode(500);
    }
  }

  [HttpGet("{id}", Name = "Get")]
  [ProducesResponseType(typeof(DataSourceDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Get(int id)
  {
    try
    {
      var source = await _dataConsumerService.GetDataSource(id);
      if (source == null)
      {
        return NotFound();
      }
      return Ok(source);
    }
    catch (Exception ex)
    {
      _logger?.LogError(ex, "Error while getting dashboard");
      return StatusCode(500);
    }
  }

  // add
  [HttpPost]
  [ProducesResponseType(typeof(DataSourceDto), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Post([FromBody] DataSourceDto source)
  {
    try
    {
      if (source == null)
      {
        return BadRequest();
      }
      var newSource = await _dataConsumerService.AddDataSource(source);
      return CreatedAtAction(nameof(Get), new { id = newSource.Id }, newSource);
    }
    catch (Exception ex)
    {
      _logger?.LogError(ex, "Error while adding dashboard");
      return StatusCode(500);
    }
  }

  // update
  [HttpPut]
  [ProducesResponseType(typeof(DataSourceDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Put([FromBody] DataSourceDto source)
  {
    try
    {
      if (source == null)
      {
        return BadRequest();
      }
      var updatedSource = await _dataConsumerService.UpdateDataSource(source);
      return Ok(updatedSource);
    }
    catch (Exception ex)
    {
      _logger?.LogError(ex, "Error while updating dashboard");
      return StatusCode(500);
    }
  }

  // delete
  [HttpDelete("{id}")]
  [ProducesResponseType(typeof(DataSourceDto), StatusCodes.Status202Accepted)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      await _dataConsumerService.DeleteDataSource(id);
      return Accepted();
    }
    catch (Exception ex)
    {
      _logger?.LogError(ex, "Error while deleting dashboard");
      return StatusCode(500);
    }
  }

 }
