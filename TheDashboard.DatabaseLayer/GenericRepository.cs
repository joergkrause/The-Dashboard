using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TheDashboard.DatabaseLayer.Domain;
using TheDashboard.DatabaseLayer.Interfaces;

namespace TheDashboard.DatabaseLayer;

public class GenericRepository<C, T, U> : IGenericRepository<T, U> 
  where C : DbContext
  where T : EntityBase<U> where U : IEquatable<U>
{

  private C _context;

  public GenericRepository(C context)
  {
    _context = context;
  }

  protected C Context { get => _context; }

  public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
  {
    var count = await _context.Set<T>().CountAsync(predicate);
    if (count > 10000)
    {
      throw new ArgumentOutOfRangeException(nameof(predicate), "Count for {predicate} excceds maximum. Consider using paged methods.");
    }

    var models = _context.Set<T>().Where(predicate).AsNoTracking();
    return await models.ToListAsync();
  }

  public async Task<T?> Single(Expression<Func<T, bool>> predicate)
  {
    return await _context.Set<T>().SingleOrDefaultAsync(predicate);
  }

  public async Task<int> Count(Expression<Func<T, bool>> predicate)
  {
    return await _context.Set<T>().CountAsync(predicate);
  }

  public async Task<bool> Upsert(T model)
  {
    _context.Entry(model).State = model.Id.Equals(default) ? EntityState.Added : EntityState.Modified;
    var result = await _context.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> Delete(T model)
  {
    _context.Entry(model).State = EntityState.Deleted;
    var result = await _context.SaveChangesAsync();
    return result > 0;
  }

}
