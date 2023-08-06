namespace TheDashboard.ViewModels.Data;

public abstract class ViewModelBase<T> where T : IEquatable<T>
{
  public T Id { get; set; } = default!;
}