using MassTransit;
using MassTransit.Transports;
using TheDashboard.BuildingBlocks.Core.EventBus;

namespace TheDashboard.UiInfoService.Infrastructure.Integration;

public record ConsumerEvent<T>(int TileId, T Data) : IntegrationEvent where T : class;
