using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Workshop.DatabaseLayer;

namespace Workshop.TileService.Infrastructure;

public class GenericRepository<T> : IGenericRepository<T> where T : IntEntityBase
{

  protected DbContext db;

  public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
  {
    var count = db.Set<T>().Count(predicate);
    if (count > 10000)
    {
      throw new ArgumentOutOfRangeException("count zu groß!");
    }

    var models = db.Set<T>().Where(predicate).AsNoTracking();
    return await models.ToListAsync();
  }

  public Task<T> Single(Expression<Func<T, bool>> predicate)
  {
    throw new NotImplementedException();
  }

  public Task<T> Count(Expression<Func<T, bool>> predicate)
  {
    throw new NotImplementedException();
  }

  public Task<bool> Upsert(T model)
  {
    db.Entry(model).State = model.Id == default ? EntityState.Added : EntityState.Modified;
    throw new NotImplementedException();
  }

  public Task<bool> Delete(T model)
  {
    db.Entry(model).State = EntityState.Deleted;
    throw new NotImplementedException();
  }


}
