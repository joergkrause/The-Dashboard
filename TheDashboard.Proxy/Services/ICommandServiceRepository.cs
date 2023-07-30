using TheDashboard.BuildingBlocks.Core.EventStore;

namespace TheDashboard.Proxy.Services
{
  public interface ICommandServiceRepository
  {
    Task StoreAndPublish<TEvent>(TEvent evt, CancellationToken cancellationToken) where TEvent : Command;
  }
}