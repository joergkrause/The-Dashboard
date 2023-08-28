using TheDashboard.SharedEntities;

namespace TheDashboard.DataSourceService.BusinessLogic
{
  public interface IDataSourceService
  {
    Task<IEnumerable<DataSourceDto>> GetAllDataSource();
    Task<DataSourceDto?> GetDataSource(int id);

    Task<DataSourceDto> AddDataSource(DataSourceDto dataSourceDto);

    // update
    Task<DataSourceDto> UpdateDataSource(DataSourceDto dataSourceDto);

    // delete
    Task DeleteDataSource(int id);
  }
}