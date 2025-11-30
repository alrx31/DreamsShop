using Domain.Entity;
using Domain.IRepositories.Base;

namespace Domain.IRepositories;

public interface IDreamCategoryRepository : IBaseRepository<DreamCategory>
{
    Task<IQueryable<DreamCategory>> GetCategoriesByDreamIdAsync(Guid dreamId, CancellationToken cancellationToken = default);
}