namespace TheDashboard.FrontendUi.Services
{
  public interface ITileDataService
  {
    event OnMessageEvent Message;

    Task Init();

    bool IsConnected
    {
      get;
    }
  }
}