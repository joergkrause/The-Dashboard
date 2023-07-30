using TheDashboard.BuildingBlocks.Core.EventStore;
using TheDashboard.Clients;

namespace TheDashboard.FrontendUi.Services;

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