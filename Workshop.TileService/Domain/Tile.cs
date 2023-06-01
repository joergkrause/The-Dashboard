using System.ComponentModel.DataAnnotations;

namespace Workshop.Domain;

public class Tile : EntityBase<Guid>
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
