using Microsoft.EntityFrameworkCore;
using Order_Services.Models;

namespace Order_Services
{
   
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Subtotal).HasPrecision(10, 2);
                entity.Property(e => e.DeliveryFee).HasPrecision(10, 2);
                entity.Property(e => e.Tax).HasPrecision(10, 2);
                entity.Property(e => e.Total).HasPrecision(10, 2);
                entity.Property(e => e.Status).HasConversion<int>();

                entity.OwnsOne(e => e.DeliveryAddress, address =>
                {
                    address.Property(a => a.Street).IsRequired().HasMaxLength(200);
                    address.Property(a => a.City).IsRequired().HasMaxLength(100);
                    address.Property(a => a.PostalCode).IsRequired().HasMaxLength(20);
                });

                entity.HasMany(e => e.Items)
                      .WithOne(i => i.Order)
                      .HasForeignKey(i => i.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MenuItemName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasPrecision(10, 2);
            });
        }


    }


}


