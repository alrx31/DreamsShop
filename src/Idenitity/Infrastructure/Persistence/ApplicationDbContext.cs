using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    // Users
    public DbSet<ConsumerUser> ConsumerUser { get; init; }
    public DbSet<ProducerUser> ProducerUser { get; init; }

    //Producer
    public DbSet<Producer> Producer { get; init; }
}