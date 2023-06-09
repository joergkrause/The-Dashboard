using AutoMapper;
using Workshop.Domain;
using Workshop.Services.TransferObjects;

namespace Workshop.Services.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<Dashboard, DashboardDto>()
      .ReverseMap();
  }
}
