namespace TheDashboard.DataConsumerService.TransferObjects;

public class DataSourceDto
{
  public int Id { get; set; }

  public string Name { get; set; } = default!;

  public string Description { get; set; } = default!;

  public string Url { get; set; } = default!;

  public Guid DashboardId { get; set; }
}
