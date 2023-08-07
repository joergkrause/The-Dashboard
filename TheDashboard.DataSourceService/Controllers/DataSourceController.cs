using Microsoft.AspNetCore.Mvc;
using TheDashboard.SharedEntities;

namespace TheDatabase.DataConsumerService.Controllers;


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
