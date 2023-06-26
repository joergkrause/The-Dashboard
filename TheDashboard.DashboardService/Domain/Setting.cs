using System.ComponentModel.DataAnnotations.Schema;

namespace TheDashboard.DashboardService.Domain;

public class Setting
{

  public DashboardType Type { get; set; }

  [NotMapped]
  public SettingDetails SettingDetail { get; set; } = new SettingDetails();

}
