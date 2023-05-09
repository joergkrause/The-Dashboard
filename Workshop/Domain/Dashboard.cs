using System.ComponentModel.DataAnnotations;

namespace Workshop.Domain;

public class Dashboard : EntityBase
{
  [Required]
  [StringLength(100, MinimumLength = 3)]
  public string Name { get; set; } = default!;

  public ICollection<Tile> Tiles { get; set; } = new HashSet<Tile>();
}
