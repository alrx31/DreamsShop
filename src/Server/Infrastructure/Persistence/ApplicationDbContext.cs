using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) {}
    
    public DbSet<ConsumerUser> ConsumerUser { get; init; }
    
    public DbSet<Dream> Dream { get; init; }
    
    public DbSet<ProducerUser> ProducerUser { get; init; }
    
    public DbSet<Producer> Producer { get; init; }
    
    public DbSet<Category> Category { get; init; }
    
    public DbSet<DreamInCategory> DreamInCategory { get; init; }
    
    public DbSet<Order> Order { get; init; }
    
    public DbSet<DreamInOrder> DreamInOrder { get; init; }
    
    public DbSet<OrderTransaction> OrderTransaction { get; init; }
    
    public DbSet<Media> Media { get; init; }
    
    public DbSet<RatingsDreams> RaitingsDreams { get; init; }
    
    public DbSet<RatingsProducer> RaitingsProducer { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Producer>()
            .HasMany(x => x.Dreams)
            .WithOne(y => y.Producer)
            .HasForeignKey(y => y.Producer_Id);

        modelBuilder.Entity<Producer>()
            .HasMany(x => x.ProducerUsers)
            .WithOne(y => y.Producer)
            .HasForeignKey(y => y.Producer_Id);
        
        modelBuilder.Entity<Producer>()
            .HasMany(x => x.Raitings_Producers)
            .WithOne(y => y.Producer)
            .HasForeignKey(y => y.Producer_Id);

        modelBuilder.Entity<ConsumerUser>()
            .HasMany(x => x.Raitings_Dreamses)
            .WithOne(y => y.ConsumerUser)
            .HasForeignKey(y => y.Consumer_Id);

        modelBuilder.Entity<ConsumerUser>()
            .HasMany(x => x.Orders)
            .WithOne(y => y.ConsumerUser)
            .HasForeignKey(y => y.Consumer_Id);

        modelBuilder.Entity<ConsumerUser>()
            .HasMany(x => x.Raitings_Producers)
            .WithOne(y => y.ConsumerUser)
            .HasForeignKey(y => y.Consumer_Id);
        
        modelBuilder.Entity<Dream>()
            .HasMany(x => x.DreamInCategories)
            .WithOne(y => y.Dream)
            .HasForeignKey(y => y.Dream_Id);
        
        modelBuilder.Entity<Dream>()
            .HasMany(x => x.DreamInOrders)
            .WithOne(y => y.Dream)
            .HasForeignKey(y => y.Dream_Id);
        
        modelBuilder.Entity<Dream>()
            .HasMany(x => x.Raitings_Dreamses)
            .WithOne(y => y.Dream)
            .HasForeignKey(y => y.Dream_Id);

        modelBuilder.Entity<Dream>()
            .HasOne(d => d.Image_Media)
            .WithOne(m => m.DreamAsImage)
            .HasForeignKey<Dream>(d => d.Image_Media_Id);
        
        modelBuilder.Entity<Dream>()
            .HasOne(d => d.Preview_Media)
            .WithOne(m => m.DreamAsPreview)
            .HasForeignKey<Dream>(d => d.Preview_Media_Id);
        
        modelBuilder.Entity<Category>()
            .HasMany(x => x.DreamInCategories)
            .WithOne(y => y.Category)
            .HasForeignKey(y => y.Category_Id);
        
        modelBuilder.Entity<Order>()
            .HasMany(x => x.DreamInOrders)
            .WithOne(y => y.Order)
            .HasForeignKey(y => y.Order_Id);

        modelBuilder.Entity<OrderTransaction>()
            .HasMany(x => x.Orders)
            .WithOne(y => y.OrderTransactions)
            .HasForeignKey(y => y.Transaction_Id);
    }
}