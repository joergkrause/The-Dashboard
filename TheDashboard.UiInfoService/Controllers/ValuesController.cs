using MassTransit.Transports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheDashboard.SharedEntities;
using TheDashboard.UiInfoService.Infrastructure.Integration;

namespace TheDashboard.UiInfoService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : UiInfoBaseController
{
  public ValuesController(IUiInfoBaseController implementation) : base(implementation)
  {
  }
}
