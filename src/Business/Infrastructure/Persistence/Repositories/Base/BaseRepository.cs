using System.Linq.Expressions;
using Domain.IRepositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Base;

public abstract class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T>
    where T : class
{
    protected readonly ApplicationDbContext Context = context;

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        return (await Context.Set<T>().AddAsync(entity, cancellationToken)).Entity;       
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        Context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task<T?> GetAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        if (ids is null || ids.Length == 0)
        {
            return Task.FromResult<T?>(null);
        }

        var keyValues = Array.ConvertAll(ids, id => (object)id);
        return Context.Set<T>().FindAsync(keyValues, cancellationToken).AsTask();
    }

    public async Task<List<K>> GetAsync<K>
    (
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, K>>? selector = null,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<T>()
        .Where(filter ?? (_ => true))
        .Select(selector ?? (e => (K)(object)e))
        .Skip(skip ?? 0)
        .Take(take ?? int.MaxValue);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().CountAsync(cancellationToken);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        Context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }
}
