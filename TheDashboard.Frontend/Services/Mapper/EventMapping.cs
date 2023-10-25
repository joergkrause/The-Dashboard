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

    // convert the viewmodels into the command objects
    CreateMap<DashboardViewModel, DashboardAdded>()
      .ForCtorParam(nameof(DashboardAdded.Id), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(DashboardAdded.Item), opt => opt.MapFrom(s => s));

    CreateMap<DashboardViewModel, DashboardRemoved>()
      .ForCtorParam(nameof(DashboardRemoved.Id), opt => opt.MapFrom(s => s.Id));

    CreateMap<DashboardViewModel, DashboardUpdated>()
      .ForCtorParam(nameof(DashboardUpdated.Id), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(DashboardUpdated.Item), opt => opt.MapFrom(s => s));

    CreateMap<TileViewModel, TileAdded>()
      .ForCtorParam(nameof(TileAdded.Id), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(TileAdded.Item), opt => opt.MapFrom(s => s));

    CreateMap<TileViewModel, TileRemoved>()
      .ForCtorParam(nameof(TileRemoved.TileId), opt => opt.MapFrom(s => s.Id));

    CreateMap<TileViewModel, TileUpdated>()
      .ForCtorParam(nameof(TileUpdated.TileId), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(TileUpdated.Item), opt => opt.MapFrom(s => s));

  }
}

