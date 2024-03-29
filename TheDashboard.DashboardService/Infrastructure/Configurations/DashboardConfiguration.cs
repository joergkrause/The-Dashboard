﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TheDashboard.DatabaseLayer;
using TheDashboard.DashboardService.Domain;
using TheDashboard.DatabaseLayer.Interfaces;
using TheDashboard.DatabaseLayer.Configurations;

namespace TheDashboard.DashboardService.Infrastructure.Configurations;

public class DashboardConfiguration : EntityTypeConfigurationDependency<Dashboard>
{

  private readonly IEncryptionService _encryptService;
  private readonly IConfiguration _configuration;

  public DashboardConfiguration(IEncryptionService encryptionService, IConfiguration configuration)
  {
    _encryptService = encryptionService;
    _configuration = configuration;
  }

  public override void Configure(EntityTypeBuilder<Dashboard> builder)
  {
    builder.ToTable("Dashboards");
    builder.HasKey(e => e.Id);
    builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false).IsRequired();
    builder.Property(e => e.Name).HasConversion(str => str, b => _encryptService.DecryptString(_configuration.GetValue<string>("Encryption:Cipher"), b));
    builder.HasIndex(e => e.Name).IsUnique();

    builder.Property(e => e.Version).HasColumnName("Ver");
    builder.Property(e => e.Theme).HasMaxLength(50).IsRequired(false);
    // Shadow Properties
    builder.Property<DateTime>(nameof(IAuditableEntityBaseProperties.CreatedAt));
    builder.Property<DateTime>(nameof(IAuditableEntityBaseProperties.ModifiedAt));
    builder.Property<string>(nameof(IAuditableEntityBaseProperties.CreatedBy)).HasMaxLength(50);
    builder.Property<string>(nameof(IAuditableEntityBaseProperties.ModifiedBy)).HasMaxLength(50);
    builder.Property<bool>("IsDeleted");
    builder.HasQueryFilter(e => EF.Property<bool>(e, "IsDeleted") == false);

    // Relations
    builder.OwnsOne<Setting>(e => e.Settings)
       .Property(e => e.Type);
    builder.OwnsOne<Setting>(e => e.Settings)
      .Property<SettingDetails>("PropertyBag").HasConversion(v => JsonConvert.SerializeObject(v), v => v == null ? null : JsonConvert.DeserializeObject<SettingDetails>(v));

    builder
      .HasOne(e => e.Layout)
      .WithMany()
      .OnDelete(DeleteBehavior.NoAction);

  }
}
