
using TheDashboard.DataConsumerService.Domain;
using TheDashboard.DataConsumerService.Infrastructure;

namespace TheDashboard.DataConsumerService.Infrastructure;

public static class SeedDatabase
{

  public static async Task Seed(DataConsumerDbContext context)
  {
    var d1 = new Dashboard
    {
      Id = Guid.NewGuid(),
      Name = "Dashboard 1",
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
