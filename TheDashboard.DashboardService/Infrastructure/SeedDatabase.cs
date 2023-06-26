using TheDashboard.DashboardService.Domain;

namespace TheDashboard.DashboardService.Infrastructure;

public static class SeedDatabase
{

  public static async Task Seed(DashboardContext context)
  {
    var seedId = new Guid("8BFE41C6-45BC-420A-8AF9-356D96B200AB");

    var l1 = new AdminLayout
    {
      XDimension = 12,
      YDimension = 12
    };

    var d1 = new Dashboard
    {
      Id = seedId,
      Name = "Dashboard 1",
      Theme = "Dark",
      Version = 1,
      IsDefault = true,
      Settings = new Setting { Type = DashboardType.User },
      Layout = l1
    };
    context.Set<Dashboard>().Add(d1);
    await context.SaveChangesAsync();

    
  }

}
