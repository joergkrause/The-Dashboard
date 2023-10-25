using AutoMapper;
using TheDashboard.SharedEntities;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.Frontend.Services.Mapper;

public class ModelMappings : Profile
{
  public ModelMappings()
  {
    CreateMap<TileDto, TileViewModel>().ReverseMap();
    CreateMap<DashboardDto, DashboardViewModel>().ReverseMap();
    CreateMap<DataSourceDto, DataSourceViewModel>().ReverseMap();
  }
}
