using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TheDashboard.DatabaseLayer.Interfaces;

namespace TheDashboard.DatabaseLayer.Interceptors;

public class CurrentUser : IUser
{
  public string Id { get; set; } = "System";
  public IIdentity User { get => new ClaimsIdentity(Id); }
}

