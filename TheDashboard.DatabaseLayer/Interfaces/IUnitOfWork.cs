namespace TheDashboard.DatabaseLayer.Interfaces;

public interface IUnitOfWork
{
    void BeginTransaction();
    void Commit();
    void Rollback();
    int SaveChanges();
    Task<int> SaveChangesAsync();
}
