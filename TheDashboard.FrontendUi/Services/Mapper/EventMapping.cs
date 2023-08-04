﻿using AutoMapper;
using TheDashboard.SharedEntities;

namespace TheDashboard.Frontend.Services.Mapper;

public class EventMapping : Profile
{
  /// <summary>
  /// This mapping takes care of converting class instances into record instances.
  /// </summary>
  public EventMapping()
  {
    CreateMap<DashboardDto, DashboardAdded>()
      .ForCtorParam(nameof(DashboardAdded.Id), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(DashboardAdded.Item), opt => opt.MapFrom(s => s));

    CreateMap<DashboardDto, DashboardRemoved>()
      .ForCtorParam(nameof(DashboardRemoved.Id), opt => opt.MapFrom(s => s.Id));

    CreateMap<DashboardDto, DashboardUpdated>()
      .ForCtorParam(nameof(DashboardUpdated.Id), opt => opt.MapFrom(s => s.Id))
      .ForCtorParam(nameof(DashboardUpdated.Item), opt => opt.MapFrom(s => s));
  }
}

