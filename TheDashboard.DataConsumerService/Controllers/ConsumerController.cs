using Microsoft.AspNetCore.Mvc;


namespace TheDatabase.DataConsumerService.Controllers;


[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(500)]
[Produces("application/json")]
public class ConsumerController : ControllerBase
{

  private readonly ILogger<ConsumerController>? _logger;

  public ConsumerController(IServiceProvider serviceProvider)
  {
    _logger = serviceProvider.GetService<ILogger<ConsumerController>>();
  }

 }
