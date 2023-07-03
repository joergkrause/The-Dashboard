using Microsoft.EntityFrameworkCore;
using Moq;
using TheDashboard.DashboardService.Domain;
using TheDashboard.DashboardService.Infrastructure;
using TheDashboard.DashboardService.Infrastructure.Configurations;
using TheDashboard.DatabaseLayer.Configurations;
using TheDashboard.DatabaseLayer.Interfaces;

namespace TheDashboard.DashboardService.Tests
{
  [TestClass]
  public class DomainModels
  {

    private const string TESTDB = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DashboardServiceTestDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    private DbContextOptions<DashboardContext> options;

    private Mock<IUser> userMock;
    private Mock<IDateTime> dateTimeMock ;
    private Mock<IEncryptionService> encryptMock ;

    private List<EntityTypeConfigurationDependency> configurations;

    [TestInitialize]
    public void Init()
    {
      var builder = new DbContextOptionsBuilder<DashboardContext>();
      builder.UseSqlServer(TESTDB, opt => opt.CommandTimeout(30));
      options = builder.Options;

      userMock = new Mock<IUser>();
      dateTimeMock = new Mock<IDateTime>();
      encryptMock = new Mock<IEncryptionService>();

      configurations = new List<EntityTypeConfigurationDependency>
      {
        new DashboardConfiguration(encryptMock.Object, null!),
        new LayoutConfiguration()
      };

      using var context = new DashboardContext(options, configurations, encryptMock.Object, userMock.Object, dateTimeMock.Object);
      context.Database.EnsureDeleted();
      context.Database.Migrate();
    }

    [TestMethod]
    public async Task CreateDashboard()
    {
      using var context = new DashboardContext(options, configurations, encryptMock.Object, userMock.Object, dateTimeMock.Object);

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
      using var context = new DashboardContext(options, configurations, encryptMock.Object, userMock.Object, dateTimeMock.Object);

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