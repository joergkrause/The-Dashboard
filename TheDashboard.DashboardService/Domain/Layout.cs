using TheDashboard.DatabaseLayer;
using TheDashboard.DatabaseLayer.Domain;

namespace TheDashboard.DashboardService.Domain;

public abstract class Layout : EntityBase<int>
{

  public int XDimension { get; set; }

  public int YDimension { get; set; }

  public ICollection<Dashboard> Dashboards { get; set; } = new HashSet<Dashboard>();
}


