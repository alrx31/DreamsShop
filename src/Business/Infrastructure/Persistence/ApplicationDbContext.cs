using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Dream> Dream { get; init; }
    
    public DbSet<Category> Category { get; init; }
    
    public DbSet<DreamCategory> DreamCategory { get; init; }
}