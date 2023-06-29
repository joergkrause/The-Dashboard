using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDashboard.DatabaseLayer.Domain;
using TheDashboard.DatabaseLayer.Interfaces;

namespace TheDashboard.DatabaseLayer.Interceptors;

public class EncryptSaveChangesInterceptor : SaveChangesInterceptor
{
  private readonly IEncryptionService _encryptionService;

  public EncryptSaveChangesInterceptor(
         IEncryptionService encryptionService)
  {
    _encryptionService = encryptionService;
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

    foreach (var entry in context.ChangeTracker.Entries<EntityBase<int>>())
    {
      if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
      {
        entry.Property("").CurrentValue = _encryptionService.EncryptString("cipher_16_chars", entry.Property("").CurrentValue.ToString());
      }
    }
  }
}

