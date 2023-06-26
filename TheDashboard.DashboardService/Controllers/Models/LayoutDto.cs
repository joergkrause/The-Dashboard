using TheDashboard.DatabaseLayer;

namespace TheDashboard.DashboardService.Domain;

public class LayoutDto
{

  public int Id { get; set; }

  public int XDimension { get; set; }

  public int YDimension { get; set; }

  public Guid DashboardId { get; set; }
}
