using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TheDashboard.Ui.Attributes;

namespace TheDashboard.ViewModels.Data;

public class DataSourceViewModel : ViewModelBase<int>
{
  [Required()]
  [StringLength(100)]
  public string Name { get; set; } = default!;

  [StringLength(512)]
  public string Description { get; set; } = default!;

  [StringLength(2000)]
  [Url]
  public string Url { get; set; } = default!;

  [StringLength(20)]
  public string SourceType { get; set; } = default!;

  public int TileId { get; set; }

}
