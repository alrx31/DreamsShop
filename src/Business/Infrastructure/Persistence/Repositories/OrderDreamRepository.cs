using Domain.Entity;
using Domain.IRepositories;
using Infrastructure.Persistence.Repositories.Base;

namespace Infrastructure.Persistence.Repositories;

public class OrderDreamRepository(ApplicationDbContext context) : BaseRepository<OrderDream>(context), IOrderDreamRepository;