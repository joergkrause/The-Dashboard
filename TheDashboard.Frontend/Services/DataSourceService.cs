using AutoMapper;
using System.Text;
using System.Text.Json;
using TheDashboard.SharedEntities;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.Frontend.Services;

public sealed class DataSourceService : ServiceInvokeCommand, IDataSourceService
{
  private readonly IDataSourceClient _datasourceClient;

  public DataSourceService(IMapper mapper, IHttpClientFactory httpClientFactory, IDataSourceClient datasourceClient) : base(mapper, httpClientFactory)
  {
    _datasourceClient = datasourceClient;
  }

  public async Task<IList<DataSourceViewModel>> GetDatasources()
  {
    var dashboards = await _datasourceClient.GetAllAsync();
    var viewModels = Mapper.Map<IList<DataSourceViewModel>>(dashboards);
    return viewModels;
  }

  public async Task<DataSourceViewModel> GetDatasource(int id)
  {
    var dashboard = await _datasourceClient.GetAsync(id);
    var viewModel = Mapper.Map<DataSourceViewModel>(dashboard);
    return viewModel;
  }
  public async Task InvokeCommand<TEvent>(DataSourceViewModel dto) where TEvent : Command
  {
    await base.InvokeCommand<TEvent, DataSourceViewModel, int>(dto);
  }

}
