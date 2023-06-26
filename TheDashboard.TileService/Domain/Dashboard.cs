using System.ComponentModel.DataAnnotations;
using TheDashboard.DatabaseLayer.Domain;

namespace TheDashboard.TileService.Domain;

public class Dashboard : EntityBase<Guid>
{
  [Required]
  [StringLength(100, MinimumLength = 3)]
  public string Name { get; set; } = default!;

  public ICollection<Tile> Tiles { get; set; } = new HashSet<Tile>();

}
