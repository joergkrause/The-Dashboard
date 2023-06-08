namespace Workshop.DatabaseLayer;

public abstract class EntityBase<T> where T : IEquatable<T>
{
  public int Id { get; set; } = default!;
}
