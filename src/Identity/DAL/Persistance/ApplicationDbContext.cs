using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistance;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; } 
    public DbSet<Producer> Producers { get; set; } 
    public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
}