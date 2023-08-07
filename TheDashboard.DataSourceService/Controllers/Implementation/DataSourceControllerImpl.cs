using TheDashboard.DataConsumerService.BusinessLogic;
using TheDashboard.SharedEntities;
using TheDatabase.DataConsumerService.Controllers;

namespace TheDashboard.DataConsumerService.Controllers.Implementation;

public class DataSourceControllerImpl : IDataSourceBaseController
{

  private readonly ILogger<DataSourceController>? _logger;
  private readonly IDataSourceService _dataConsumerService;

  public DataSourceControllerImpl(IServiceProvider serviceProvider)
  {
    _logger = serviceProvider.GetService<ILogger<DataSourceController>>();
    _dataConsumerService = serviceProvider.GetRequiredService<IDataSourceService>();
  }

  public async Task<ICollection<DataSourceDto>> GetAllAsync()
  {
    var sources = await _dataConsumerService.GetAllDataSource();
    return sources.ToList();
  }

  public async Task<DataSourceDto> GetAsync(int id)
  {
    var source = await _dataConsumerService.GetDataSource(id);
    return source;
  }

}
