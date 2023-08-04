using Microsoft.AspNetCore.Mvc;
using TheDashboard.Controllers;
using TheDashboard.Services;
using TheDashboard.SharedEntities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheDashboard.DashboardService.Controllers.Implementation;

[Route("api/[controller]")]
[ApiController]
public class DashboardControllerImpl : IDashboardBaseController
{

  private readonly ILogger<DashboardController>? _logger;
  private readonly IDashboardService _dashboardService;

  public DashboardControllerImpl(IServiceProvider serviceProvider)
  {
    _logger = serviceProvider.GetService<ILogger<DashboardController>>();
    _dashboardService = serviceProvider.GetRequiredService<IDashboardService>();

  }

  public async Task<ICollection<DashboardDto>> GetAllAsync()
  {
    _logger?.LogInformation("Get called");
    var models = await _dashboardService.GetDashboards(e => e.Layout);
    return models.ToList();
  }

  public async Task<DashboardDto> GetAsync(Guid id)
  {
    var model = await _dashboardService.GetDashboard(id);
    return model;
  }

  public async Task SearchAsync([FromQuery] string name)
  {
    throw new NotImplementedException();
  }

  public async Task<DashboardDto> AddDashboardAsync([FromBody] DashboardDto value)
  {
      var model = await _dashboardService.AddDashboard(value);
      return model;
  }

  public async Task UpdateDashboardAsync(int id, [FromBody] DashboardDto value)
  {
      await _dashboardService.UpdateDashboard(value);
  }

  public async Task RemoveDashboardAsync(Guid id)
  {
    await _dashboardService.DeleteDashboard(id);
  }

}
