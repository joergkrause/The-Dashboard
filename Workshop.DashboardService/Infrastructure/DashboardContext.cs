using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using Workshop.DashboardService.Infrastructure.Configuration;
using Workshop.DatabaseLayer;
using Workshop.Domain;

namespace Workshop.DashboardService.Infrastructure;

public class DashboardContext : DbContext
{

  public static readonly ILoggerFactory SqlLogger = LoggerFactory.Create(builder => { builder.AddConsole(); });

  public DashboardContext(DbContextOptions<DashboardContext> options) : base(options)
  {

  }

  public DbSet<Dashboard> Dashboards { get; set; }
  public DbSet<Layout> Layouts { get; set; }
  public DbSet<Tile> Tiles { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseLoggerFactory(SqlLogger);
    optionsBuilder.EnableDetailedErrors();
    optionsBuilder.EnableSensitiveDataLogging();
    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfiguration(new DashboardConfiguration());

    // modelBuilder.Entity<Setting>();

    modelBuilder.Entity<Layout>().ToTable("Layouts");
    modelBuilder.Entity<Tile>().ToTable("Tiles");
    modelBuilder.Entity<Tile>().Property(e => e.DashboardId).IsRequired(false);
    // modelBuilder.Entity<Tile>().Property(e => e.TileId).HasColumnType("");

    modelBuilder.AddInboxStateEntity();
    modelBuilder.AddOutboxMessageEntity();
    modelBuilder.AddOutboxStateEntity();
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
