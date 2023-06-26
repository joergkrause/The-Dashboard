using TheDashboard.DatabaseLayer;
using TheDashboard.DatabaseLayer.Domain;

namespace TheDashboard.TileService.Domain;

public class Visualizer : NamedEntity<int>
{

  public Kind Type { get; set; }

  public uint Refreshrate { get; set; }

  public bool Interactive { get; set; }

  public Transformer? Transformer { get; set; }


  public ICollection<Tile> Tiles { get; set; } = new HashSet<Tile>();
}
