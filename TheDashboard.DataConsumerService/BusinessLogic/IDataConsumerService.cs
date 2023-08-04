using TheDashboard.DataConsumerService.Domain;
using TheDashboard.DataConsumerService.Infrastructure;
using TheDashboard.SharedEntities;

namespace TheDashboard.DataConsumerService.BusinessLogic
{
  public interface IDataConsumerService
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