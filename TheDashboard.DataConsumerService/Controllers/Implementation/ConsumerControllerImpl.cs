using TheDashboard.DataConsumerService.BusinessLogic;
using TheDashboard.SharedEntities;
using TheDatabase.DataConsumerService.Controllers;

namespace TheDashboard.DataConsumerService.Controllers.Implementation;

public class ConsumerControllerImpl : IDataConsumerBaseController
{

  private readonly ILogger<ConsumerController>? _logger;
  private readonly IDataConsumerService _dataConsumerService;

  public ConsumerControllerImpl(IServiceProvider serviceProvider)
  {
    _logger = serviceProvider.GetService<ILogger<ConsumerController>>();
    _dataConsumerService = serviceProvider.GetRequiredService<IDataConsumerService>();
  }

  public async Task<DataSourceDto> AddAsync(DataSourceDto source)
  {
    var newSource = await _dataConsumerService.AddDataSource(source);
    return newSource;
  }

  public Task<DataSourceDto> DeleteAsync(int id)
  {
    throw new NotImplementedException();
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

  public async Task<DataSourceDto> UpdateAsync(DataSourceDto source)
  {
    var updated = await _dataConsumerService.UpdateDataSource(source);
    return updated;
  }
}
