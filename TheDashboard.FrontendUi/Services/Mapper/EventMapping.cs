using AutoMapper;
using TheDashboard.BuildingBlocks.Core.EventStore;
using TheDashboard.Clients;

namespace TheDashboard.FrontendUi.Services.Mapper;

public class EventMapping : Profile
{
  /// <summary>
  /// This mapping takes care of converting class instances into record instances.
  /// </summary>
  public EventMapping()
  {
    CreateMap<DashboardDto, DashboardAdded>()
      .ForCtorParam(nameof(DashboardAdded.Id), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(DashboardAdded.Name), opt => opt.MapFrom(s => s.Name));

    CreateMap<DashboardDto, DashboardRemoved>()
      .ForCtorParam(nameof(DashboardRemoved.Id), opt => opt.MapFrom(s => s.Id));

    CreateMap<DashboardDto, DashboardUpdated>()
      .ForCtorParam(nameof(DashboardUpdated.Id), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(DashboardUpdated.Name), opt => opt.MapFrom(s => s.Name));
  }
}

