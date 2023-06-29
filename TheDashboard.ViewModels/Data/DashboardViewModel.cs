using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TheDashboard.Ui.Attributes;

namespace TheDashboard.ViewModels.Data;

public class DashboardViewModel : ViewModelBase<Guid>
{
    [Required()]
    [StringLength(100)]
    public string Name { get; set; } = default!;

    [StringLength(512)]
    public string Description { get; set; } = default!;


}
