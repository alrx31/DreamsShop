using Domain.Entity;
using Domain.IRepositories;
using Infrastructure.Persistence.Repositories.Base;

namespace Infrastructure.Persistence.Repositories;

public class CategoryRepository(ApplicationDbContext context) : BaseRepository<Category>(context), ICategoryRepository;