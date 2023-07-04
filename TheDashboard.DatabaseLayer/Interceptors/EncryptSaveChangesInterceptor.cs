using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TheDashboard.DatabaseLayer.Attributes;
using TheDashboard.DatabaseLayer.Domain;
using TheDashboard.DatabaseLayer.Interfaces;

namespace TheDashboard.DatabaseLayer.Interceptors;

public class EncryptSaveChangesInterceptor : SaveChangesInterceptor
{
  private readonly IEncryptionService _encryptionService;
  private readonly IConfiguration _configuration;

  public EncryptSaveChangesInterceptor(
         IEncryptionService encryptionService,
         IConfiguration configuration
    )
  {
    _encryptionService = encryptionService;
    _configuration = configuration;
  }

  public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
  {
    EncryptEntities(eventData.Context);

    return base.SavingChanges(eventData, result);
  }

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
  {
    EncryptEntities(eventData.Context);

    return base.SavingChangesAsync(eventData, result, cancellationToken);
  }

  public void EncryptEntities(DbContext? context)
  {
    if (context == null) return;
    var cipher = _configuration["Encryption:Cipher"];
    if (cipher == null) return;
    foreach (var entry in context.ChangeTracker.Entries<EntityBase<int>>())
    {
      if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
      {
        // get properties that have Encrypt attribute
        var properties = entry.Properties.Where(p => p.Metadata?.PropertyInfo?.GetCustomAttribute<EncryptAttribute>() != null);
        foreach (var property in properties)
        {
          property.CurrentValue = _encryptionService.EncryptString(cipher, property?.CurrentValue?.ToString()!);
        }
      }
    }
  }
}

