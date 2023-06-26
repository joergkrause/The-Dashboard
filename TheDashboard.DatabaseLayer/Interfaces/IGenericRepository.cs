using System.Linq.Expressions;
using TheDashboard.DatabaseLayer.Domain;

namespace TheDashboard.DatabaseLayer.Interfaces
{
  public interface IGenericRepository<T, U> where T : EntityBase<U> where U : IEquatable<U>
  {
    Task<int> Count(Expression<Func<T, bool>> predicate);
    Task<bool> Delete(T model);
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    Task<T?> Single(Expression<Func<T, bool>> predicate);
    Task<bool> Upsert(T model);
  }
}