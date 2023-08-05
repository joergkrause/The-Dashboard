using TheDashboard.SharedEntities;

namespace TheDashboard.Frontend.Services;

public interface ITileDataService
{

    Task InvokeCommand<TEvent>(TileDto dto) where TEvent : Command;

    event OnMessageEvent Message;

    Task Init();

    bool IsConnected
    {
        get;
    }
}