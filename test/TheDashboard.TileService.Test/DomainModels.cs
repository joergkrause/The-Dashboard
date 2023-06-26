using Microsoft.EntityFrameworkCore;
using TheDashboard.TileService.Domain;
using TheDashboard.TileService.Infrastructure;

namespace TheDashboard.TileService.Test
{
  [TestClass]
  public class DomainModels
  {
    private const string TESTDB = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TileServiceTestDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    private DbContextOptions<TileDbContext> options;

    [TestInitialize]
    public void Init()
    {
      var builder = new DbContextOptionsBuilder<TileDbContext>();
      builder.UseSqlServer(TESTDB, opt => opt.CommandTimeout(30));
      options = builder.Options;

      using var context = new TileDbContext(options);
      context.Database.EnsureDeleted();
      context.Database.Migrate();
    }

    [TestMethod]
    public async Task CreateDashboard()
    {
      var context = new TileDbContext(options);

      context.Set<Dashboard>().Add(new Dashboard
      {
        Id = Guid.NewGuid(),
        Name = "Test Dashboard",
        Tiles = new List<Tile>
        {
          new Tile { Name = "Test Tile 1", Description = "Test Type 1", Title = "T1", SubTitle = "Sub 1" },
          new Tile { Name = "Test Tile 2", Description = "Test Type 2", Title = "T2", SubTitle = "Sub 2" },
        }
      });

      await context.SaveChangesAsync();

      var result = context.Set<Dashboard>().Include(e => e.Tiles).ToList();

      Assert.AreEqual(1, result.Count);

      Assert.AreEqual(2, result.Single().Tiles.Count);

    }

    

  }
}