using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Dream> Dream { get; init; }
    public DbSet<Category> Category { get; init; }
    public DbSet<DreamCategory> DreamCategory { get; init; }
    public DbSet<UserDream> UserDream { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<DreamCategory>()
            .HasKey(c => new { c.DreamId, c.CategoryId });
        
        builder.Entity<UserDream>()
            .HasKey(c => new { c.UserId, c.DreamId });
        
    }
}