using System.Linq.Expressions;

namespace Workshop.DatabaseLayer
{
  public interface IGenericRepository<T> where T : IntEntityBase
  {
    Task<T> Count(Expression<Func<T, bool>> predicate);
    Task<bool> Delete(T model);
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    Task<T> Single(Expression<Func<T, bool>> predicate);
    Task<bool> Upsert(T model);
  }
}