using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Workshop.Controllers
{
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
}
