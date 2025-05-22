using Domain.Entity;
using Domain.IRepositories.Base;

namespace Domain.IRepositories;

public interface IOrderTransactionRepository : ICrudRepository<OrderTransaction>
{
}