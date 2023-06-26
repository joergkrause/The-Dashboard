﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proxy;

namespace FrontendApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DashboardController : ControllerBase
  {
    private readonly DashboardProxy _proxyService;

    public DashboardController(DashboardProxy dashboardProxy)
    {
      _proxyService = dashboardProxy;
    }

    [HttpGet("dashboard")]
    public async Task<IEnumerable<DashboardDto>> GetDashboards()
    {
      var dashboards = await _proxyService.GetAllAsync();
      return dashboards;
    }


  }
}


