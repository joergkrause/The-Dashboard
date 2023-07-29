using System.ComponentModel.DataAnnotations;
using TheDashboard.Clients;

namespace TheDashboard.FrontendUi.EventSourcing;

public record DashboardAdded(Guid Id, string Name);

public record DashboardRemoved(Guid Id);

public record DashboardUpdated(Guid Id, string Name);
