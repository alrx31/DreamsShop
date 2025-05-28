using Domain.Entity;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ConsumerUser> ConsumerUser { get; init; }
    public DbSet<ProducerUser> ProducerUser { get; init; }
    public DbSet<RefreshTokenModel> RefreshTokens { get; init; }
}