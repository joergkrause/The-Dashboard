using AutoMapper;
using MassTransit;
using TheDashboard.DataConsumerService.BusinessLogic;
using TheDashboard.SharedEntities;

namespace TheDashboard.DataConsumerService.Infrastructure.Integration;

public class DataSourceCreatedHandler : IConsumer<DataSourceAdded>
{

  private readonly IMapper _mapper;
  private readonly IDataSourceService _dataSourceService;

  public DataSourceCreatedHandler(IMapper mapper, IDataSourceService dataSourceService)
  {
    _mapper = mapper;
    _dataSourceService = dataSourceService;
  }


  public async Task Consume(ConsumeContext<DataSourceAdded> context)
  {
    var id = context.Message.DataSourceId;    
    var ds = await _dataSourceService.GetDataSource(id);
    if (ds == null)
    {
      ds = new() { Id = id, Name = context.Message.Item.Name };
      await _dataSourceService.AddDataSource(ds);
    }
  }
}
