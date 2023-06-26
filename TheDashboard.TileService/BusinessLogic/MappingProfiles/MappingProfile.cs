using AutoMapper;
using TheDashboard.TileService.Controllers.Models;
using TheDashboard.TileService.Domain;

namespace TheDashboard.TileService.BusinessLogic.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tile, TileDto>()
          .ForMember(e => e.DashboardId, o => o.MapFrom(e => e.Dashboard!.Id))
          .ReverseMap();
    }
}
