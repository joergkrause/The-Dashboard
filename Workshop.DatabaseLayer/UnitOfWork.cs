using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Workshop.DatabaseLayer;

public abstract class UnitOfWork<T> : IUnitOfWork where T: DbContext
{

  public UnitOfWork(T context)
  {
    Context = context;
  }
  protected T Context { get; }

  public int SaveChanges()
  {
    return Context.SaveChanges();
  }

  public Task<int> SaveChangesAsync()
  {
    return Context.SaveChangesAsync();
  }

  private IDbContextTransaction _transaction;

  public void BeginTransaction()
  {
    _transaction = Context.Database.BeginTransaction();
  }

  public void Commit()
  {
    try
    {
      SaveChanges();
      _transaction.Commit();
    }
    finally
    {
      _transaction.Dispose();
    }
  }

  public void Rollback()
  {
    _transaction.Rollback();
    _transaction.Dispose();
  }

}
