using System.ComponentModel.DataAnnotations;
using TheDashboard.DatabaseLayer.Domain;

namespace TheDashboard.TileService.Domain;

public class Tile : NamedEntity<int>
{
  public string Title { get; set; } = default!;

  public string SubTitle { get; set; } = default!;

  public string StaticText { get; set; } = default!;

  public string Icon { get; set; } = default!;

  public Guid DataSource { get; set; }

  public bool IsActive { get; set; }

  public Dashboard Dashboard { get; set; } = default!;

  public Visualizer? Visualizer { get; set; }


}
