using Domain.Entity;
using Domain.IRepositories.Base;

namespace Domain.IRepositories;

public interface ICategoryRepository : ICrudRepository<Category>, IGetAllRepository<Category>
{
}