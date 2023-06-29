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

  public Position Position { get; set; } = new Position();

}

public class Position
{
  public int XOffset { get; set; }

  public int YOffset { get; set; }

  public int Width { get; set; } = 3;

  public int Height { get; set; } = 2;
}