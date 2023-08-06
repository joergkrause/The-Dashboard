using TheDashboard.TileService.Domain;
using TheDashboard.TileService.Infrastructure;

namespace TheDashboard.TileService.Infrastructure;

public static class SeedDatabase
{

  public static async Task Seed(TileDbContext context)
  {

    var hasSeed = context.Set<Dashboard>().Any(e => e.Id == Guid.Parse("8BFE41C6-45BC-420A-8AF9-356D96B200AB"));
    if (hasSeed) return;

    var seedId = new Guid("8BFE41C6-45BC-420A-8AF9-356D96B200AB");

    var d1 = new Dashboard
    {
      Id = seedId,
      Name = "Demo Dashboard 1",
    };
    var t1 = new Tile
    {
      Title = "Demo Tile 1",
      SubTitle = "More text here...",
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
      },
      Dashboard = d1
    };

    context.Set<Tile>().Add(t1);
    context.Set<Dashboard>().Add(d1);

    await context.SaveChangesAsync();
  }

}
