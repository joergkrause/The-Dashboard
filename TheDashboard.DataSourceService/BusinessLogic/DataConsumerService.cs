﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheDashboard.DataSourceService.Domain;
using TheDashboard.DataSourceService.Infrastructure;
using TheDashboard.SharedEntities;

namespace TheDashboard.DataSourceService.BusinessLogic;

public class DataSourceService : IDataSourceService
{

  private readonly ILogger<DashboardService> _logger;
  private readonly DataSourceDbContext _dataconsumerDbContext;
  private readonly IMapper _mapper;

  public DataSourceService(ILogger<DashboardService> logger, DataSourceDbContext tileDbContext, IMapper mapper)
  {
    _logger = logger;
    _dataconsumerDbContext = tileDbContext;
    _mapper = mapper;
  }

  public async Task<DataSourceDto?> GetDataSource(int id)
  {
    var model = await _dataconsumerDbContext.Set<DataSource>().SingleOrDefaultAsync(e => e.Id == id);
    if (model == null)
    {
      return null;
    }
    return _mapper.Map<DataSourceDto>(model);
  }

  public async Task<IEnumerable<DataSourceDto>> GetAllDataSource()
  {
    var model = await _dataconsumerDbContext.Set<DataSource>().ToListAsync();
    return _mapper.Map<IEnumerable<DataSourceDto>>(model);
  }

  // add
  public async Task<DataSourceDto> AddDataSource(DataSourceDto dataSourceDto)
  {
    var model = _mapper.Map<DataSource>(dataSourceDto);
    _dataconsumerDbContext.Set<DataSource>().Add(model);
    await _dataconsumerDbContext.SaveChangesAsync();
    return _mapper.Map<DataSourceDto>(model);
  }

  // update
  public async Task<DataSourceDto> UpdateDataSource(DataSourceDto dataSourceDto)
  {
    var model = _mapper.Map<DataSource>(dataSourceDto);
    _dataconsumerDbContext.Set<DataSource>().Update(model);
    await _dataconsumerDbContext.SaveChangesAsync();
    return _mapper.Map<DataSourceDto>(model);
  }

  // delete
  public async Task DeleteDataSource(int id)
  {
    var model = await _dataconsumerDbContext.Set<DataSource>().SingleOrDefaultAsync(e => e.Id == id);
    if (model != null)
    {
      _dataconsumerDbContext.Set<DataSource>().Remove(model);
      await _dataconsumerDbContext.SaveChangesAsync();
    }
  }

  public async Task SendMessageForSource(int id, object message)
  {
    // publish message through rabbit mq


  }

}
