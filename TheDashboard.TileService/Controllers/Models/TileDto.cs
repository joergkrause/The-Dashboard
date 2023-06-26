namespace TheDashboard.TileService.Controllers.Models;

public class TileDto
{
  public int Id { get; set; }
  public string Title { get; set; } = default!;

  public string Url { get; set; } = default!;

  public Guid DashboardId { get; set; }
}
