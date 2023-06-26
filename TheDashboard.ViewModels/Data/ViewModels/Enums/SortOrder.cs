using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDashboard.ViewModels.Data.ViewModels.Enums;

public enum SortOrder
{
  Asc,
  Desc,
  None
}

public enum DataAction
{
  Add,
  Remove,
  Create,
  Read,
  Update,
  Delete
}

public enum ClaimUsage
{
  Input,
  Display,
  Output
}
