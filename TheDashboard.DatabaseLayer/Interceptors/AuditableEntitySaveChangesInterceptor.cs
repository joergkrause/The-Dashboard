using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDashboard.DatabaseLayer.Interfaces;

namespace TheDashboard.DatabaseLayer.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
  private readonly IUser _user;
  private readonly IDateTime _dateTime;

  public AuditableEntitySaveChangesInterceptor(
      IUser user,
      IDateTime dateTime)
  {
    _user = user;
    _dateTime = dateTime;
  }

  public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
  {
    UpdateEntities(eventData.Context);

    return base.SavingChanges(eventData, result);
  }

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
  {
    UpdateEntities(eventData.Context);

    return base.SavingChangesAsync(eventData, result, cancellationToken);
  }

  public void UpdateEntities(DbContext? context)
  {
    if (context == null) return;

    foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
    {
      if (entry.State == EntityState.Added)
      {
        entry.Property("CreatedBy").CurrentValue = _user.User.Name;
        entry.Property("Created").CurrentValue = _dateTime.CurrentUtc;
      }

      if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
      {
        entry.Property("LastModifiedBy").CurrentValue = _user.User.Name;
        entry.Property("LastModified").CurrentValue = _dateTime.CurrentUtc;
      }
    }
  }
}

public static class Extensions
{
  public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
      entry.References.Any(r =>
          r.TargetEntry != null &&
          r.TargetEntry.Metadata.IsOwned() &&
          (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}