namespace Workshop.Services.TransferObjects;

public class TileDto
{
  public Guid Id { get; set; }
  public string Title { get; set; }

  public string Url { get; set; }

  public Guid DashboardId { get; set; }
}
