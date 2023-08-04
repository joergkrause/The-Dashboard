using AutoMapper;
using TheDashboard.SharedEntities;
using TheDashboard.TileService.Domain;

namespace TheDashboard.TileService.BusinessLogic.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tile, TileDto>()
          .ForMember(e => e.DashboardId, o => o.MapFrom(e => e.Dashboard!.Id))
          .ForMember(e => e.VisualizerId, o => o.MapFrom(e => e.Visualizer!.Id))
          .ForMember(e => e.DataSourceId, o => o.MapFrom(e => e.DataSource))
          .ForMember(e => e.XOffset, o => o.MapFrom(e => e.Position.XOffset))
          .ForMember(e => e.YOffset, o => o.MapFrom(e => e.Position.YOffset))
          .ForMember(e => e.Width, o => o.MapFrom(e => e.Position.Width))
          .ForMember(e => e.Height, o => o.MapFrom(e => e.Position.Height));
    }
}
