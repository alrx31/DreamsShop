using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories;

public abstract class BaseRepositoryTest
{
    protected readonly ApplicationDbContext Context;

    protected BaseRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        Context = new ApplicationDbContext(options);
    }
}