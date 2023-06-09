using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Workshop.DatabaseLayer;

namespace Workshop.Domain;

public class Dashboard : EntityBase<Guid>, IAuditableEntityBase, ISoftDeleteEntityBase
{
  [Encrypt]
  public string Name { get; set; } = default!;

  // char(3)
  public string Theme { get; set; } = default!;

  public int Version { get; set; }

  public bool IsDefault { get; set; }

  public Layout Layout { get; set; } // 1:1

  public Setting Settings { get; set; } // Owned by

  public ICollection<Tile> Tiles { get; set; } = new HashSet<Tile>(); // 1:n

  // V1
  //public bool HasTiles()
  //{
  //  return Tiles.Any();
  //}

  // V2
  [NotMapped]
  public bool HasTiles { get => Tiles.Any(); }

  [NotMapped]
  public bool HasActiveTiles { get => Tiles.Any(t => t.IsActive); }

}

public class Setting
{

  public DashboardType Type { get; set; }

  [NotMapped]
  public SettingDetails SettingDetail { get; set; }
  
}

public class SettingDetails
{
  public int DefaultX { get; set; }

  public int DefaultY { get; set; }

  // 35 weitere...
}

public abstract class Layout : EntityBase<int>
{
  public int XDimension { get; set; }

  public int YDimension { get; set; }

  public Guid DashboardId { get; set; } // 1:1
}

public class AdminLayout : Layout
{
  
}

public class UserLayout : Layout
{
  public DateTime ValidTo { get; set; }

  public DateTime ValidFrom { get; set; }
}

public class Tile : EntityBase<int>
{
  public int TileId { get; set; }

  public bool IsActive { get; set; }

  //public Dashboard Dashboard { get; set; } // n:1
  public Guid? DashboardId { get; set; }
}
// Layout
// Layouts
// Tile


