using AutoMapper;
using Workshop.Domain;
using Workshop.Services.TransferObjects;

namespace Workshop.Services.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<Tile, TileDto>()
      .ForMember(e => e.DashboardId, o => o.MapFrom(e => e.Dashboard!.Id))
      .ReverseMap();
  }
}
