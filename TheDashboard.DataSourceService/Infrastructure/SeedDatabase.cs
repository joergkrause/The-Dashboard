using TheDashboard.DataSourceService.Domain;

namespace TheDashboard.DataSourceService.Infrastructure;

public static class SeedDatabase
{

  public static async Task Seed(DataSourceDbContext context)
  {

    var hasSeed = context.Set<Dashboard>().Any(e => e.Id == Guid.Parse("8BFE41C6-45BC-420A-8AF9-356D96B200AB"));
    if (hasSeed) return;

    var seedId = new Guid("8BFE41C6-45BC-420A-8AF9-356D96B200AB");

    var d1 = new Dashboard
    {
      Id = seedId,
      Name = "Demo Dashboard 1",
    };
    var t1 = new HttpDataSource
    {
      Name = "Source 1",
      Description = "Init Source",
      Dashboard = d1
    };

    context.Set<DataSource>().Add(t1);
    context.Set<Dashboard>().Add(d1);

    await context.SaveChangesAsync();
  }

}
