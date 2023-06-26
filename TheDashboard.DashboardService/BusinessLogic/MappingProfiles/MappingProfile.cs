using AutoMapper;
using TheDashboard.DashboardService.Domain;
using TheDashboard.Services.TransferObjects;

namespace TheDashboard.Services.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<Dashboard, DashboardDto>()
      .ForMember(e => e.LayoutKind, opt => opt.MapFrom(e => e.Layout.GetType().Name));

    CreateMap<DashboardDto, Dashboard>()
      .ForMember(e => e.Layout, opt => opt.Ignore());

    CreateMap<Layout, LayoutDto>().ReverseMap();

  }
}
