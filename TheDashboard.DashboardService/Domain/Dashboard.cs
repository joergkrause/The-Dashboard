using TheDashboard.DatabaseLayer.Attributes;
using TheDashboard.DatabaseLayer.Domain;
using TheDashboard.DatabaseLayer.Interfaces;

namespace TheDashboard.DashboardService.Domain;

public class Dashboard : EntityBase<Guid>, IAuditableEntityBase, ISoftDeleteEntityBase
{
  [Encrypt]
  public string Name { get; set; } = default!;

  public string Theme { get; set; } = default!;

  public int Version { get; set; }

  public bool IsDefault { get; set; }

  public Layout Layout { get; set; } = default!; // 1:1

  public Setting Settings { get; set; } = new Setting(); // Owned by

}
