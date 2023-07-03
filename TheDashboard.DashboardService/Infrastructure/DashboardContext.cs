using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheDashboard.DashboardService.Domain;
using TheDashboard.DashboardService.Infrastructure.Configurations;
using TheDashboard.DatabaseLayer;
using TheDashboard.DatabaseLayer.Attributes;
using TheDashboard.DatabaseLayer.Configurations;
using TheDashboard.DatabaseLayer.Interceptors;
using TheDashboard.DatabaseLayer.Interfaces;

namespace TheDashboard.DashboardService.Infrastructure;

public class DashboardContext : DbContext
{

  public static readonly ILoggerFactory SqlLogger = LoggerFactory.Create(builder => { builder.AddConsole(); });

  private readonly IEnumerable<EntityTypeConfigurationDependency> _configurations;

  private readonly ILogger<DashboardContext> _logger;
  private readonly IEncryptionService _encryptionService;
  private readonly IUser _user;
  private readonly IDateTime _datetime;

  public DashboardContext(
    ILogger<DashboardContext> logger,
    DbContextOptions<DashboardContext> options,
    IEnumerable<EntityTypeConfigurationDependency> configurations,
    IEncryptionService encryptionService, 
    IUser user, 
    IDateTime dateTime) : base(options)
  {
    base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    _logger = logger;
    _configurations = configurations;
    _encryptionService = encryptionService;
    _user = user;
    _datetime = dateTime;

    _logger?.LogDebug("******************************* _configurations " + _configurations ==  null ? "NOTHING": _configurations.Count().ToString());
  }

  public DbSet<Dashboard> Dashboards { get; set; } = default!;
  public DbSet<Layout> Layouts { get; set; } = default!;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {    
    optionsBuilder.UseLoggerFactory(SqlLogger);
    optionsBuilder.EnableDetailedErrors();
    optionsBuilder.EnableSensitiveDataLogging();

    optionsBuilder.AddInterceptors(
      new EncryptSaveChangesInterceptor(_encryptionService),      
      new AuditableEntitySaveChangesInterceptor(_user, _datetime)
    );

    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    if (_configurations != null)
    {
      foreach (var entityTypeConfiguration in _configurations)
      {
        entityTypeConfiguration.Configure(modelBuilder);
      }
    }

    modelBuilder.Entity<AdminLayout>().ToTable("Layouts");
    modelBuilder.Entity<UserLayout>().ToTable("Layouts");

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
    return base.SaveChangesAsync(cancellationToken);
  }

}
