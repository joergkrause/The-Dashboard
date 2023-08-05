using Newtonsoft.Json.Linq;
using TheDashboard.SharedEntities;
using TheDashboard.UiInfoService.Infrastructure.Integration;

namespace TheDashboard.UiInfoService.Controllers.Implementation;

public class ValuesControllerImpl : IUiInfoBaseController
{

  private readonly ILogger<ValuesController> _logger;
  private readonly ConsumerHandler _consumerHandler;

  public ValuesControllerImpl(ILogger<ValuesController> logger, ConsumerHandler consumerHandler)
  {
    _logger = logger;
    _consumerHandler = consumerHandler;
  }

  public async Task ValueAsync(int id, string value)
  {
    var evt = new DataEvent(value);
    await _consumerHandler.ConsumeTest(evt);
  }
}
