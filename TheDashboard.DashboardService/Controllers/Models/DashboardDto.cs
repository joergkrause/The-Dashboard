using System.ComponentModel.DataAnnotations;

namespace TheDashboard.Services.TransferObjects;

public class DashboardDto
{
  public Guid Id { get; set; }

  [Required]
  [StringLength(100, MinimumLength = 3)]
  public string Name { get; set; } = default!;

  public string LayoutKind { get; set; } = default!;

}
