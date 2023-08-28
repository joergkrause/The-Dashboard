using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TheDashboard.DataSourceService.Domain;

namespace TheDashboard.DataSourceService.Infrastructure;

public class DataConsumerDbContext : DbContext
{
  public DataConsumerDbContext(DbContextOptions<DataConsumerDbContext> options) : base(options)
  {
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
    optionsBuilder.UseLoggerFactory(null);
    optionsBuilder.EnableSensitiveDataLogging(false);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {

    modelBuilder.Entity<Dashboard>().ToTable("Dashboards");
    modelBuilder.Entity<Dashboard>().Property(e => e.Name).HasMaxLength(100).IsUnicode(true).IsRequired();
    modelBuilder.Entity<Dashboard>()
      .HasMany(e => e.DataSources)
      .WithOne(e => e.Dashboard)
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<DataSource>().ToTable("DataSources");
    modelBuilder.Entity<DataSource>().Property(e => e.Name).HasMaxLength(100).IsUnicode(true).IsRequired();
    modelBuilder.Entity<DataSource>().Property(e => e.Description).HasMaxLength(512).IsRequired(false);
    modelBuilder.Entity<DataSource>().Property(e => e.Url).HasMaxLength(512).IsRequired(false);
    modelBuilder.Entity<DataSource>().Ignore(e => e.Kind);
    modelBuilder.Entity<DataSource>().HasDiscriminator(e => e.Kind).HasValue<HttpDataSource>(Kind.Http);

    modelBuilder.Entity<HttpDataSource>().ToTable("DataSources");

    modelBuilder.AddInboxStateEntity();
    modelBuilder.AddOutboxMessageEntity();
    modelBuilder.AddOutboxStateEntity();
  }
}
