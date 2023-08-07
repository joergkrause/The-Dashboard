using System.ComponentModel.DataAnnotations.Schema;
using TheDashboard.DatabaseLayer.Domain;

namespace TheDashboard.DataConsumerService.Domain;

public abstract class DataSource : NamedEntity<int>
{

  public Dashboard Dashboard { get; set; } = default!;

  public virtual Kind Kind { get; set; }

  public string Url { get; set; } = default!;

  public bool Authenticated { get; set; }

}

public class HttpDataSource : DataSource
{

  [NotMapped]
  public override Kind Kind { get => Kind.Http; }

  public string? Method { get; set; }

  public string? Body { get; set; }

  public string? Headers { get; set; }

  public string? Query { get; set; }

}

