using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using Workshop.DatabaseLayer;

namespace Workshop.DashboardService.Infrastructure;

public class DashboardContext : DbContext
{

  public static readonly ILoggerFactory SqlLogger = LoggerFactory.Create(builder => { builder.AddConsole(); });

  public DashboardContext(DbContextOptions<DashboardContext> options) : base(options)
  {

  }


  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseLoggerFactory(SqlLogger);
    optionsBuilder.EnableDetailedErrors();
    optionsBuilder.EnableSensitiveDataLogging();
    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // TODO: Mapping    
    base.OnModelCreating(modelBuilder);
  }

  public override int SaveChanges()
  {
    throw new NotImplementedException("Use SaveChangesAsync instead!");
  }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    SaveInterceptor();
    CryptoInterceptor();
    return base.SaveChangesAsync(cancellationToken);
  }

  private void SaveInterceptor()
  {
    var entries = ChangeTracker.Entries<IAuditableEntityBase>();
    foreach (var entry in entries)
    {
      switch (entry.State)
      {
        case EntityState.Modified:
          entry.Property(nameof(IAuditableEntityBaseProperties.ModifiedAt)).CurrentValue = DateTime.UtcNow;
          break;
        case EntityState.Added:
          entry.Property(nameof(IAuditableEntityBaseProperties.CreatedAt)).CurrentValue = DateTime.UtcNow;
          break;
      }
    }
    var entriesToDelete = ChangeTracker.Entries<ISoftDeleteEntityBase>().Where(e => e.State == EntityState.Deleted);
    foreach(var entry in entriesToDelete)
    {
      entry.Property("IsDeleted").CurrentValue = true;
    }
  }

  private void CryptoInterceptor()
  {
    // Look for properties with EncryptAttribute and encrypt.
    foreach (var item in ChangeTracker
      .Entries() // no filter due to Identity Models
      .Where(item => item.State == EntityState.Added || item.State == EntityState.Modified)
      )
    {
      foreach (var property in item.Entity.GetType().GetProperties())
      {
        var toEncrypt = property.GetCustomAttributes(true).OfType<EncryptAttribute>().Any();
        if (!toEncrypt)
        {
          continue;
        }
        var val = item.Property(property.Name).CurrentValue?.ToString();
        if (val != null)
        {
          var enc = AesOperation.EncryptString("0123456789123456", val);
          item.Property(property.Name).CurrentValue = enc;
        }
      }
    }
  }

}
