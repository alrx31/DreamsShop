using Domain.Entity;
using Domain.IRepositories;
using Infrastructure.Persistence.Repositories.Base;

namespace Infrastructure.Persistence.Repositories;

public class DreamCategoryRepository(ApplicationDbContext context) : BaseRepository<DreamCategory>(context), IDreamCategoryRepository
{
    public Task<IQueryable<DreamCategory>> GetCategoriesByDreamIdAsync(Guid dreamId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Context.DreamCategory.Where(x => x.DreamId == dreamId).Distinct());
    }
}