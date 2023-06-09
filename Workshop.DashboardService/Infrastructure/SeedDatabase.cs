using Workshop.Domain;

namespace Workshop.DashboardService.Infrastructure;

public static class SeedDatabase
{

  public static async Task Seed(DashboardContext context)
  {
    var d1 = new Dashboard
    {
      Name = "Dashboard 1",
      Theme = "Dark",
      Version = 1,
      IsDefault = true,
      Settings = new Setting { Type = DashboardType.Regular }
    };
    // V1
    // context.Dashboards.Add(d1);
    // V2
    context.Set<Dashboard>().Add(d1);

    await context.SaveChangesAsync();

    var d2 = context.Dashboards.First();

    d2.Name = "Dashboard 3";

    await context.SaveChangesAsync();
    
  }

}
