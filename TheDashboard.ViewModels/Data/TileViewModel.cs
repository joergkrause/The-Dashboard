using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDashboard.ViewModels.Data;

public class TileViewModel : ViewModelBase<int>
{
  public string Title { get; set; } = default!;

  public string SubTitle { get; set; } = default!;

  public string StaticText { get; set; } = default!;

  public string Icon { get; set; } = default!;

  public Guid DataSource { get; set; }

  public bool IsActive { get; set; }

  public Guid? DashboardId { get; set; } = default!;

  public int? VisualizerId { get; set; }

  public int XOffset { get; set; }

  public int YOffset { get; set; }

  public int Width { get; set; } = 3;

}
