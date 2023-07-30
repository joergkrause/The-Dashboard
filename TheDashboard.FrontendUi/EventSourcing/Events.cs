using System.ComponentModel.DataAnnotations;
using TheDashboard.BuildingBlocks.Core.EventStore;
using TheDashboard.Clients;

namespace TheDashboard.FrontendUi.EventSourcing;

public record DashboardAdded(Guid Id, string Name): Command;

public record DashboardRemoved(Guid Id) : Command;

public record DashboardUpdated(Guid Id, string Name) : Command;
