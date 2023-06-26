using System.ComponentModel.DataAnnotations;

namespace TheDashboard.ViewModels.Data.ViewModels;

public class AddressViewModel : ViewModelBase<int>
{

  [Required, StringLength(50)]
  public string Street { get; set; } = string.Empty;

  [Required, StringLength(80)]
  public string City { get; set; } = string.Empty;

  [Required, StringLength(50)]
  public string State { get; set; } = string.Empty;

  [Required, StringLength(8)]
  public string PostalCode { get; set; } = string.Empty;

  [Required, StringLength(60)]
  public string Country { get; set; } = string.Empty;

  [Phone, StringLength(50)]
  public string Phone { get; set; } = string.Empty;

}