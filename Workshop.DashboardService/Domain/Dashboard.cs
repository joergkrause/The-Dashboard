using System.ComponentModel.DataAnnotations;
using Workshop.DatabaseLayer;

namespace Workshop.Domain;

public class Dashboard : EntityBase<Guid>, IAuditableEntityBase, ISoftDeleteEntityBase
{
  [Encrypt]
  [Required]
  [StringLength(100, MinimumLength = 3)]
  public string Name { get; set; } = default!;

  public string Theme { get; set; } = default!;

  public int Version { get; set; }
  

}

public class Settings
{
  // PropertyBag
}

// Layout
  // Layouts
// Tile
 

