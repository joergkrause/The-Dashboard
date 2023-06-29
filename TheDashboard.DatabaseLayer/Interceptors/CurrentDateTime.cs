using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDashboard.DatabaseLayer.Interfaces;

namespace TheDashboard.DatabaseLayer.Interceptors;

public class CurrentDateTime : IDateTime
{
  private DateTime dateStore;

  public CurrentDateTime()
  {
    dateStore = DateTime.UtcNow;
  }

  public DateTime CurrentUtc { get => dateStore; set => dateStore = value; }
}
