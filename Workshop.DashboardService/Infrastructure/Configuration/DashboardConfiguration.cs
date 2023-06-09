using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workshop.DatabaseLayer;
using Workshop.Domain;

namespace Workshop.DashboardService.Infrastructure.Configuration;

public class DashboardConfiguration : IEntityTypeConfiguration<Dashboard>
{
  public void Configure(EntityTypeBuilder<Dashboard> builder)
  {
    builder.ToTable("Dashboards");
    builder.HasKey(e => e.Id);
    builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false).IsRequired();
    builder.HasIndex(e => e.Name).IsUnique();
    builder.Property(e => e.Version).HasColumnName("VER");
    builder.Property(e => e.Theme).HasMaxLength(50).IsRequired(false);
    // Shadow Properties
    builder.Property<DateTime>(nameof(IAuditableEntityBaseProperties.CreatedAt));
    builder.Property<DateTime>(nameof(IAuditableEntityBaseProperties.ModifiedAt));
    builder.Property<string>(nameof(IAuditableEntityBaseProperties.CreatedBy)).HasMaxLength(50);
    builder.Property<string>(nameof(IAuditableEntityBaseProperties.ModifiedBy)).HasMaxLength(50);

    // Relations
    builder.OwnsOne<Setting>(e => e.Settings)
       .Property(e => e.Type);
    builder.HasMany(e => e.Tiles)
       .WithOne()
       // .HasForeignKey("DashboardId")
       .OnDelete(DeleteBehavior.Cascade);
    //builder.HasOne(e => e.Layout)
    //  .WithOne()
    //  .HasForeignKey("DashboardId")
    //  .OnDelete(DeleteBehavior.Cascade);
  }
}
