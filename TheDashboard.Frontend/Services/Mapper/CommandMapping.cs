using AutoMapper;
using Blazorise;
using TheDashboard.SharedEntities;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.Frontend.Services.Mapper;

public class CommandMapping : Profile
{
  /// <summary>
  /// This mapping takes care of converting class instances into record instances.
  /// 
  /// The UI handles ViewModels only, and from here we create the necessary command objects.
  /// </summary>
  public CommandMapping()
  {
    CreateMap<DashboardViewModel, AddDashboard>()
      .ForCtorParam(nameof(AddDashboard.Id), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(AddDashboard.Item), opt => opt.MapFrom(s => s));

    CreateMap<DashboardViewModel, RemoveDashboard>()
      .ForCtorParam(nameof(RemoveDashboard.Id), opt => opt.MapFrom(s => s.Id));

    CreateMap<DashboardViewModel, UpdateDashboard>()
      .ForCtorParam(nameof(UpdateDashboard.Id), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(UpdateDashboard.Item), opt => opt.MapFrom(s => s));

    CreateMap<TileViewModel, AddTile>()
    .ForCtorParam(nameof(AddTile.Id), opt => opt.MapFrom(s => s.Id))
    .ForCtorParam(nameof(AddTile.Item), opt => opt.MapFrom(s => s));

    CreateMap<TileViewModel, RemoveTile>()
      .ForCtorParam(nameof(RemoveTile.TileId), opt => opt.MapFrom(s => s.Id));

    CreateMap<TileViewModel, UpdateTile>()
      .ForCtorParam(nameof(UpdateTile.TileId), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(UpdateTile.Item), opt => opt.MapFrom(s => s));
  }
}

