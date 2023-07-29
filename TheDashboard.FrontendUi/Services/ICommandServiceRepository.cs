namespace TheDashboard.FrontendUi.Services
{
  public interface ICommandServiceRepository
  {
    Task StoreAndPublish<TDto, TEvent>(TDto dto, CancellationToken cancellationToken);
  }
}