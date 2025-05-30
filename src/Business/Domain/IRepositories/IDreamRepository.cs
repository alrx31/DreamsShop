using Domain.Entity;
using Domain.IRepositories.Base;

namespace Domain.IRepositories;

public interface IDreamRepository : ICrudRepository<Dream>, IPaginationRepository<Dream>
{
}