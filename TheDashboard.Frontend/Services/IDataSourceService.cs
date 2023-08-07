using TheDashboard.SharedEntities;
using TheDashboard.ViewModels.Data;

namespace TheDashboard.Frontend.Services
{
  public interface IDataSourceService
  {
    Task<DataSourceViewModel> GetDatasource(int id);
    Task<IList<DataSourceViewModel>> GetDatasources();
    Task InvokeCommand<TEvent>(DataSourceViewModel dto) where TEvent : Command;
  }
}