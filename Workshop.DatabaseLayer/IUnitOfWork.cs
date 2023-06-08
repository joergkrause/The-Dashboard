namespace Workshop.DatabaseLayer;

public interface IUnitOfWork
{
  void BeginTransaction();
  void Commit();
  void Rollback();
  int SaveChanges();
  Task<int> SaveChangesAsync();
}
