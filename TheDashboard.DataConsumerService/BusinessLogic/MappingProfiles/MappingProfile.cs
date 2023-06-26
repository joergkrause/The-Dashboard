using AutoMapper;
using TheDashboard.DataConsumerService.Domain;
using TheDashboard.DataConsumerService.TransferObjects;

namespace TheDatabase.DataConsumerService.BusinessLogic.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Dashboard, DashboardDto>()
          .ReverseMap();
    }
}
