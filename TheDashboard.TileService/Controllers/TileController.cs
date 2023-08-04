using Microsoft.AspNetCore.Mvc;
using TheDashboard.SharedEntities;
using TheDashboard.TileService.BusinessLogic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheDashboard.TileService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TileController : TileBaseController
{

  public TileController(ITileBaseController implementation) : base(implementation)
  {

  }

}
