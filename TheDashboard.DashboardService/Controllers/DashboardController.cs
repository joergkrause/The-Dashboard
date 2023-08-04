using Microsoft.AspNetCore.Mvc;
using TheDashboard.SharedEntities;

namespace TheDashboard.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class DashboardController : DashboardBaseController
{

  public DashboardController(IDashboardBaseController implementation) : base(implementation)
  {

  }

}
