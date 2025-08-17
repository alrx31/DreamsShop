using Domain.Entity;
using Domain.IRepositories;

namespace Infrastructure.Persistence.Repositories;

public class ProducerRepository(ApplicationDbContext context) : IProducerRepository
{
    public async Task AddAsync(Producer entity, CancellationToken cancellationToken = default)
    {
        await context.Producer.AddAsync(entity, cancellationToken);
    }

    public Task DeleteAsync(Producer entity, CancellationToken cancellationToken = default)
    {
        context.Producer.Remove(entity);

        return Task.CompletedTask;   
    }

    public async Task<Producer?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Producer.FindAsync(id, cancellationToken);
    }

    public Task UpdateAsync(Producer entity, CancellationToken cancellationToken = default)
    {
        context.Producer.Update(entity);
        
        return Task.CompletedTask;
    }
}
