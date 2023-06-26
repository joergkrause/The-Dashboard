using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheDashboard.ViewModels.Data.ViewModels;

public class CustomerViewModel : ViewModelBase<int>
{

  [Required]
  [StringLength(100)]
  public string Name { get; set; }

  public AddressViewModel Address { get; set; }

  [Required]
  [EmailAddress]
  public string Email { get; set; }

  [Phone]
  public string PhoneNumber { get; set; } = string.Empty;

  public ICollection<DashboardViewModel> Projects { get; set; } = new HashSet<DashboardViewModel>();

}
