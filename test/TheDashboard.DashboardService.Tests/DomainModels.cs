using Microsoft.EntityFrameworkCore;
using TheDashboard.DashboardService.Domain;
using TheDashboard.DashboardService.Infrastructure;

namespace TheDashboard.DashboardService.Tests
{
  [TestClass]
  public class DomainModels
  {

    private const string TESTDB = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DashboardServiceTestDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    private DbContextOptions<DashboardContext> options;

    [TestInitialize]
    public void Init()
    {
      var builder = new DbContextOptionsBuilder<DashboardContext>();
      builder.UseSqlServer(TESTDB, opt => opt.CommandTimeout(30));
      options = builder.Options;

      using var context = new DashboardContext(options);
      context.Database.EnsureDeleted();
      context.Database.Migrate();
    }

    [TestMethod]
    public async Task CreateDashboard()
    {      
      var context = new DashboardContext(options);

      context.Set<Dashboard>().Add(new Dashboard
      {
        Id = Guid.NewGuid(),
        Name = "Test Dashboard",
        Theme = "Dark",
        Version = 1,
        IsDefault = true,
        Layout = new UserLayout { YDimension = 12, XDimension = 12 },
        Settings = new Setting()
      });

      await context.SaveChangesAsync();

      var result = context.Set<Dashboard>().ToList();

      Assert.AreEqual(1, result.Count);

    }

    [TestMethod]
    public async Task CreateDashboardAndLayout()
    {
      var context = new DashboardContext(options);

      context.Set<Dashboard>().Add(new Dashboard
      {
        Id = Guid.NewGuid(),
        Name = "Test Dashboard",
        Theme = "Dark",
        Version = 1,
        IsDefault = true,
        Layout = new UserLayout { YDimension = 12, XDimension = 12 },
        Settings = new Setting(),        
      });

      await context.SaveChangesAsync();

      var result = context.Set<Dashboard>().Include(e => e.Layout).ToList();

      Assert.AreEqual(12, result.Single().Layout.YDimension);
      Assert.AreEqual(12, result.Single().Layout.XDimension);

    }

  }
}