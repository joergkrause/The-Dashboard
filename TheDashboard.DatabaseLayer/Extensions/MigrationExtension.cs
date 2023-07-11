using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TheDashboard.DatabaseLayer.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TheDashboard.DatabaseLayer.Extensions;

public static class MigrationExtension
{
  public static async Task ExecuteMigration<TContext, TSeedCheck, TKey>(this WebApplication app, Action<TContext, bool>? seedFunc = null)
    where TContext : DbContext
    where TSeedCheck : EntityBase<TKey>
    where TKey : IEquatable<TKey>
  {
    using var scope = app.Services.CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<TContext>();
    bool newDatabase = !context.Database.GetService<IRelationalDatabaseCreator>().Exists();
    await context.Database.MigrateAsync();
    var hasData = await context.Set<TSeedCheck>().AnyAsync();
    seedFunc?.Invoke(context, newDatabase);
  }

}
