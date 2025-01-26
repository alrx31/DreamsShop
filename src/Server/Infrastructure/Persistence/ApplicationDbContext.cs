using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
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
    public DbSet<RatingsDreams> RatingsDreams { get; init; }
    public DbSet<RatingsProducer> RatingsProducer { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Producer>()
            .HasMany(x => x.Dreams)
            .WithOne(y => y.Producer)
            .HasForeignKey(y => y.ProducerId);

        modelBuilder.Entity<Producer>()
            .HasMany(x => x.ProducerUsers)
            .WithOne(y => y.Producer)
            .HasForeignKey(y => y.ProducerId);
        
        modelBuilder.Entity<Producer>()
            .HasMany(x => x.Raitings_Producers)
            .WithOne(y => y.Producer)
            .HasForeignKey(y => y.ProducerId);

        modelBuilder.Entity<ConsumerUser>()
            .HasMany(x => x.RaitingsDreamses)
            .WithOne(y => y.ConsumerUser)
            .HasForeignKey(y => y.ConsumerId);

        modelBuilder.Entity<ConsumerUser>()
            .HasMany(x => x.Orders)
            .WithOne(y => y.ConsumerUser)
            .HasForeignKey(y => y.Consumer_Id);

        modelBuilder.Entity<ConsumerUser>()
            .HasMany(x => x.RaitingsProducers)
            .WithOne(y => y.ConsumerUser)
            .HasForeignKey(y => y.ConsumerId);
        
        modelBuilder.Entity<Dream>()
            .HasMany(x => x.DreamInCategories)
            .WithOne(y => y.Dream)
            .HasForeignKey(y => y.DreamId);
        
        modelBuilder.Entity<Dream>()
            .HasMany(x => x.DreamInOrders)
            .WithOne(y => y.Dream)
            .HasForeignKey(y => y.Dream_Id);
        
        modelBuilder.Entity<Dream>()
            .HasMany(x => x.Raitings_Dreamses)
            .WithOne(y => y.Dream)
            .HasForeignKey(y => y.DreamId);

        modelBuilder.Entity<Dream>()
            .HasOne(d => d.ImageMedia)
            .WithOne(m => m.DreamAsImage)
            .HasForeignKey<Dream>(d => d.ImageMediaId);
        
        modelBuilder.Entity<Dream>()
            .HasOne(d => d.PreviewMedia)
            .WithOne(m => m.DreamAsPreview)
            .HasForeignKey<Dream>(d => d.PreviewMediaId);
        
        modelBuilder.Entity<Category>()
            .HasMany(x => x.DreamInCategories)
            .WithOne(y => y.Category)
            .HasForeignKey(y => y.CategoryId);
        
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