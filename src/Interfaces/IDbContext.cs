
/*****************************************
 * 
 *  Copyright       :   © VEXIT® 2022, www.vexit.com
 *  Author          :   Vex Tatarevic
 *  Date Created    :   2022-08-14
 *  
 *  Description     :   Provides interface for DbContext from Entity Framework Core. 
 *                      This is the interface that Microsoft has not provided but we need it for certain reusable library functions and unit testing
 *  
 *  Updates         :   2025-04-19  - Vex   - Commented out EntityFrameworkCore.Internal interfaces to get rid of the warnings
 * 
 *****************************************/



using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
// using Microsoft.EntityFrameworkCore.Internal;

namespace Vexit.DataAccess.Interfaces;

public interface IDbContext : IInfrastructure<IServiceProvider>
    // The following interfaces are internal to EF Core and cause compiler warnings
    // They are used internally by EF Core itself and are not meant to be used directly by applications
    // If you need to use them, you'll need to use #pragma warning disable directives
    //, IDbContextDependencies
    //, IDbSetCache
    //, IDbContextPoolable
    //, IResettableService
    , IDisposable
    , IAsyncDisposable
{

    DatabaseFacade Database { get; }
    ChangeTracker ChangeTracker { get; }
    EntityEntry Add(object entity);
    EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
    ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default(CancellationToken));
    ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class;
    void AddRange(IEnumerable<object> entities);
    void AddRange(params object[] entities);
    Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken));
    Task AddRangeAsync(params object[] entities);
    EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;
    EntityEntry Attach(object entity);
    void AttachRange(params object[] entities);
    void AttachRange(IEnumerable<object> entities);
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    EntityEntry Entry(object entity);
    bool Equals(object obj);
    object? Find(Type entityType, params object?[]? keyValues);
    TEntity? Find<TEntity>(params object?[]? keyValues) where TEntity : class;
    ValueTask<TEntity?> FindAsync<TEntity>(params object?[]? keyValues) where TEntity : class;
    ValueTask<object?> FindAsync(Type entityType, object?[]? keyValues, CancellationToken cancellationToken);
    ValueTask<TEntity?> FindAsync<TEntity>(object?[]? keyValues, CancellationToken cancellationToken) where TEntity : class;
    ValueTask<object?> FindAsync(Type entityType, params object?[]? keyValues);
    int GetHashCode();
    EntityEntry Remove(object entity);
    EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
    void RemoveRange(IEnumerable<object> entities);
    void RemoveRange(params object[] entities);
    int SaveChanges(bool acceptAllChangesOnSuccess);
    int SaveChanges();
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    string? ToString();
    EntityEntry Update(object entity);
    EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
    void UpdateRange(params object[] entities);
    void UpdateRange(IEnumerable<object> entities);
}
