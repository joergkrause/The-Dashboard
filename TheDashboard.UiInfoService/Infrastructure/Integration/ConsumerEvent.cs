using MassTransit;
using MassTransit.Transports;
using TheDashboard.BuildingBlocks.Core.EventBus;
using TheDashboard.UiInfoService.Hubs;
using TheDashboard.UiInfoService.Infrastructure.Integration.Events;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public record ConsumerEvent<T>(int TileId, T Data) : IntegrationEvent where T : class, new();
