namespace Workshop.Services.TransferObjects;

public class TileDto
{
  public Guid Id { get; set; }
  public string Title { get; set; } = default!;

  public string Url { get; set; } = default!;

  public Guid DashboardId { get; set; }
}
