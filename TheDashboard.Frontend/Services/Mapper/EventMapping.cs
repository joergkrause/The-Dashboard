using AutoMapper;
using TheDashboard.SharedEntities;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.Frontend.Services.Mapper;

public class EventMapping : Profile
{
  /// <summary>
  /// This mapping takes care of converting class instances into record instances.
  /// 
  /// The UI handles ViewModels only, and from here we create the necessary command objects.
  /// </summary>
  public EventMapping()
  {
    CreateMap<DashboardViewModel, DashboardAdded>()
      .ForCtorParam(nameof(DashboardAdded.Id), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(DashboardAdded.Item), opt => opt.MapFrom(s => s));

    CreateMap<DashboardViewModel, DashboardRemoved>()
      .ForCtorParam(nameof(DashboardRemoved.Id), opt => opt.MapFrom(s => s.Id));

    CreateMap<DashboardViewModel, DashboardUpdated>()
      .ForCtorParam(nameof(DashboardUpdated.Id), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(DashboardUpdated.Item), opt => opt.MapFrom(s => s));
  }
}

