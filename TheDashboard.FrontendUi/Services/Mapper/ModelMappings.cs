using AutoMapper;
using TheDashboard.Clients;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.FrontendUi.Services.Mapper;

public class ModelMappings : Profile
{
    public ModelMappings()
    {
        CreateMap<TileDto, TileViewModel>();
    }
}
