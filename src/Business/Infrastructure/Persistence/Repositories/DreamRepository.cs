using Domain.Entity;
using Domain.IRepositories;
using Infrastructure.Persistence.Repositories.Base;

namespace Infrastructure.Persistence.Repositories;

public class DreamRepository(ApplicationDbContext context) : BaseRepository<Dream>(context), IDreamRepository;