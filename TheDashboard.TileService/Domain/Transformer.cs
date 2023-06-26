using TheDashboard.DatabaseLayer;
using TheDashboard.DatabaseLayer.Domain;

namespace TheDashboard.TileService.Domain;

public class Transformer : NamedEntity<int>
{
  public string Template { get; set; } = default!;

  public ICollection<Visualizer> Visualizers { get; set; } = new HashSet<Visualizer>();
}