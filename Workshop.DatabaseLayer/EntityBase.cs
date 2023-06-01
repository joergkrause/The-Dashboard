namespace Workshop.Domain;

public abstract class EntityBase<T> where T : IEquatable<T>
{
  public T Id { get; set; } = default!;
}
