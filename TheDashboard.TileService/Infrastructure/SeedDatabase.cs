using TheDashboard.TileService.Domain;
using TheDashboard.TileService.Infrastructure;

namespace TheDashboard.TileService.Infrastructure;

public static class SeedDatabase
{

  public static async Task Seed(TileDbContext context)
  {
    var d1 = new Dashboard
    {
      Id = Guid.NewGuid(),
      Name = "Dashboard 1",
    };
    var t1 = new Tile
    {
      Name = "Tile 1",
      Description = "Init Tile",
      StaticText = "Demo",
      DataSource = Guid.NewGuid(),
      Visualizer = new Visualizer
      {
        Name = "Visualizer 1",
        Type = Kind.Text,
        Refreshrate = 1000,
        Interactive = false,
        Transformer = new Transformer
        {
          Name = "Transformer 1",
          Description = "The Transformer"
        }
      }
    };

    t1.Dashboard = d1;

    context.Set<Tile>().Add(t1);
    context.Set<Dashboard>().Add(d1);

    await context.SaveChangesAsync();   
  }

}
