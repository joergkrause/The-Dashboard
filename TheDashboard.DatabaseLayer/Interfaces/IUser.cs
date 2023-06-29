using System.Security.Principal;

namespace TheDashboard.DatabaseLayer.Interfaces;

public interface IUser
{
  public IIdentity User { get; }
}