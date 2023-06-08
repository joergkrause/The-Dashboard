using System.ComponentModel.DataAnnotations;
using Workshop.DatabaseLayer;

namespace Workshop.Domain;

public class Dashboard : EntityBase<Guid>
{
  [Required]
  [StringLength(100, MinimumLength = 3)]
  public string Name { get; set; } = default!;
  
}
