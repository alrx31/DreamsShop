using Domain.Entity;
using Domain.IRepositories.Base;
using Domain.Model;

namespace Domain.IRepositories;

public interface IRefreshTokerRepository : ICrudRepository<RefreshTokenModel>
{
}