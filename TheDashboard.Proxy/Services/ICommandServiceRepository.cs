using TheDashboard.SharedEntities;

namespace TheDashboard.Proxy.Services
{
  public interface ICommandServiceRepository
  {
    Task StoreAndPublish<TEvent>(TEvent evt, CancellationToken cancellationToken) where TEvent : Command;
  }
}