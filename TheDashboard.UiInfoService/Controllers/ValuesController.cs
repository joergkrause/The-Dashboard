using MassTransit.Transports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheDashboard.DatabaseLayer.Domain.Contracts;
using TheDashboard.DataConsumerService.Infrastructure.Integration;
using TheDashboard.UiInfoService.Infrastructure.Integration;
using TheDashboard.UiInfoService.Infrastructure.Integration.Models;

namespace TheDashboard.UiInfoService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{

  private readonly ILogger<ValuesController> _logger;
  private readonly ConsumerHandler<DataConsumerMessage> _consumerHandler;

  public ValuesController(ILogger<ValuesController> logger, ConsumerHandler<DataConsumerMessage> consumerHandler)
  {
    _logger = logger;
    _consumerHandler = consumerHandler;
  }

  // Add Value to Hub
  [HttpPost("{tileId:int}")]
  [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Post([FromRoute] int tileId, [FromBody] string value)
  {
    if (ModelState.IsValid)
    {
      var evt = new DataEvent(value);
      await _consumerHandler.ConsumeTest(evt);
      return Ok();
    }
    return BadRequest(ModelState);
  }

}
