using AutoMapper;
using TheDashboard.DataSourceService.Domain;
using TheDashboard.SharedEntities;

namespace TheDashboard.DataSourceService.BusinessLogic.MappingProfiles;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<Dashboard, DashboardDto>()
      .ReverseMap();
    CreateMap<DataSource, DataSourceDto>()
      .IncludeAllDerived()
      .ForMember(t => t.SourceType, opt => opt.MapFrom(d => d.GetType().Name));

    CreateMap<DataSourceDto, DataSource>()
      .ConvertUsing((src, dest, context) =>
      {
        return src.SourceType switch
        {
          nameof(HttpDataSource) => context.Mapper.Map<HttpDataSource>(src),
          // Add more cases if you have more concrete source types...
          _ => throw new NotSupportedException($"Unsupported type {src.SourceType}"),
        };
      });

    CreateMap<DataSourceDto, HttpDataSource>();
  }
}
