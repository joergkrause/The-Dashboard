using TheDashboard.DataSourceService.BusinessLogic;
using TheDashboard.SharedEntities;

namespace TheDashboard.DataSourceService.Controllers.Implementation;

public class DataSourceControllerImpl : IDataSourceBaseController
{

  private readonly ILogger<DataSourceController>? _logger;
  private readonly IDataSourceService _dataSourceService;

  public DataSourceControllerImpl(IServiceProvider serviceProvider)
  {
    _logger = serviceProvider.GetService<ILogger<DataSourceController>>();
    _dataSourceService = serviceProvider.GetRequiredService<IDataSourceService>();
  }

  public async Task<ICollection<DataSourceDto>> GetAllAsync()
  {
    var sources = await _dataSourceService.GetAllDataSource();
    return sources.ToList();
  }

  public async Task<DataSourceDto> GetAsync(int id)
  {
    var source = await _dataSourceService.GetDataSource(id);
    return source;
  }

}
