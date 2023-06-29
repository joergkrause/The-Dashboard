using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TheDashboard.TileService.Domain;

namespace TheDashboard.TileService.Infrastructure;

public class TileDbContext : DbContext
{
  public static readonly ILoggerFactory SqlLogger = LoggerFactory.Create(builder => { builder.AddConsole(); });

  public TileDbContext(DbContextOptions<TileDbContext> options) : base(options)
  {
    base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
  }

  public DbSet<Dashboard> Dashboards { get; set; } = default!;
  public DbSet<Tile> Tiles { get; set; } = default!;

  public DbSet<Transformer> Transformers { get; set; } = default!;

  public DbSet<Visualizer> Visualizers { get; set; } = default!;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Dashboard>().ToTable("Dashboards");
    modelBuilder.Entity<Dashboard>().Property(e => e.Name).HasMaxLength(100).IsUnicode(true).IsRequired();
    modelBuilder.Entity<Dashboard>()
      .HasMany(e => e.Tiles)
      .WithOne(e => e.Dashboard)
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Tile>().ToTable("Tiles");
    modelBuilder.Entity<Tile>().Property(e => e.Name).HasMaxLength(100).IsRequired();
    modelBuilder.Entity<Tile>().Property(e => e.Description).HasMaxLength(512).IsRequired();
    modelBuilder.Entity<Tile>().Property(e => e.Title).HasMaxLength(80).IsRequired();
    modelBuilder.Entity<Tile>().Property(e => e.Icon).HasMaxLength(256).IsRequired(false);
    modelBuilder.Entity<Tile>().Property(e => e.StaticText).HasMaxLength(512).IsRequired(false);
    modelBuilder.Entity<Tile>().Property(e => e.SubTitle).HasMaxLength(120).IsRequired();
    modelBuilder.Entity<Tile>()
      .HasOne(e => e.Visualizer)
      .WithMany(e => e.Tiles)
      .OnDelete(DeleteBehavior.SetNull);

    modelBuilder.Entity<Tile>()
      .OwnsOne(e => e.Position);      

    modelBuilder.Entity<Transformer>().ToTable("Transformers");
    modelBuilder.Entity<Transformer>().Property(e => e.Name).HasMaxLength(100).IsRequired();
    modelBuilder.Entity<Transformer>().Property(e => e.Description).HasMaxLength(512).IsRequired(false);
    modelBuilder.Entity<Transformer>().Property(e => e.Template).IsRequired(false);

    modelBuilder.Entity<Visualizer>().ToTable("Visualizers");
    modelBuilder.Entity<Visualizer>().Property(e => e.Name).HasMaxLength(100).IsRequired();
    modelBuilder.Entity<Visualizer>().Property(e => e.Description).HasMaxLength(512).IsRequired(false);
    modelBuilder.Entity<Visualizer>()
      .HasMany(e => e.Tiles)
      .WithOne(e => e.Visualizer)
      .OnDelete(DeleteBehavior.SetNull);

    modelBuilder.Entity<Visualizer>()
      .HasOne(e => e.Transformer)
      .WithMany(e => e.Visualizers)
      .OnDelete(DeleteBehavior.SetNull);

    modelBuilder.AddInboxStateEntity();
  }
}
