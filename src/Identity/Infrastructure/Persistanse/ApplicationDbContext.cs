using Microsoft.EntityFrameworkCore;
using Domain.Entities;
namespace Infrastructure.Persistanse;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
    
    public DbSet<Category> Categories { get; }
    public DbSet<Dream> Dreams { get; }
    public DbSet<DreamInCategory> DreamsInCategories { get; }
    public DbSet<DreamInOrder> DreamsInOrders { get; }
    public DbSet<Media> Medias { get; }
    public DbSet<Order> Orders { get; }
    public DbSet<OrderTransaction> OrderTransactions { get; }
    public DbSet<Produser> Produsers { get; }
    public DbSet<ProduserUser> ProduserUsers { get; }
    public DbSet<ConsumerUser> ConsumerUsers { get; }
    
    
}