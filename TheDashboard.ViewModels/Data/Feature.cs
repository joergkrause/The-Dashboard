namespace TheDashboard.ViewModels.Data;

public class Feature
{
  public int Id { get; set; }
  public string Name { get; set; } = default!;
  public string Description { get; set; } = default!;
  public string Icon { get; set; } = default!;
  public string Link { get; set; } = default!;

  public bool IsActive { get; set; }
}
