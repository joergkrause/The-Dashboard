using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TheDashboard.DatabaseLayer;
using TheDashboard.DashboardService.Domain;
using System.Reflection.Emit;

namespace TheDashboard.DashboardService.Infrastructure.Configurations;

public class LayoutConfiguration : IEntityTypeConfiguration<Layout>
{
  public void Configure(EntityTypeBuilder<Layout> builder)
  {
    builder.ToTable("Layouts")
      .HasDiscriminator<int>("LayoutType")
      .HasValue<UserLayout>(1)
      .HasValue<AdminLayout>(2);
  }
}
