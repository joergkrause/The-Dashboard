using System.ComponentModel.DataAnnotations;
using Workshop.DatabaseLayer;

namespace Workshop.Domain;

public class Tile : IntEntityBase
{
  [Required]
  [StringLength(100, MinimumLength = 2)]
  public string Title { get; set; } = default!;

  [Required]
  [Url]
  public string Url { get; set; } = default!;

  public Dashboard? Dashboard { get; set; }
  //  public Guid DashboardId { get; set; }
}
