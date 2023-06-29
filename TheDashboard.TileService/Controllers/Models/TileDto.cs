namespace TheDashboard.TileService.Controllers.Models;

public class TileDto
{
  public int Id { get; set; }
  public string Title { get; set; } = default!;

  public string SubTitle { get; set; } = default!;
  
  public string Url { get; set; } = default!;

  public Guid DashboardId { get; set; }

  public Guid DataSourceId { get; set; }

  public int VisualizerId { get; set; }

  public int XOffset { get; set; }

  public int YOffset { get; set; }

  public int Width { get; set; } = 3;

  public int Height { get; set; } = 2;
}
