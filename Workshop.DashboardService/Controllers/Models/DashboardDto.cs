using System.ComponentModel.DataAnnotations;

namespace Workshop.Services.TransferObjects;

public class DashboardDto
{
  public int Id { get; set; }

  [Required]
  [StringLength(100, MinimumLength = 3)]
  public string Name { get; set; } = default!;
}
