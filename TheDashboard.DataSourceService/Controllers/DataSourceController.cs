using Microsoft.AspNetCore.Mvc;
using TheDashboard.SharedEntities;

namespace TheDashboard.DataSourceService.Controllers;


[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[Produces("application/json")]
public class DataSourceController : DataSourceBaseController
{

  public DataSourceController(IDataSourceBaseController implementation) : base(implementation)
  {

  }

}
