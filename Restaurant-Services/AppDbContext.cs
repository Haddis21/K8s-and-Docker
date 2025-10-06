using Microsoft.EntityFrameworkCore;
using Restaurant_Services.Models;

namespace Restaurant_Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Restaurant> Restaurants => Set<Restaurant>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Restaurant configuration
            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DeliveryFee).HasPrecision(10, 2);
                entity.OwnsOne(e => e.Address, address =>
                {
                    address.Property(a => a.Street).IsRequired().HasMaxLength(200);
                    address.Property(a => a.City).IsRequired().HasMaxLength(100);
                    address.Property(a => a.PostalCode).IsRequired().HasMaxLength(20);
                });

                entity.HasMany(e => e.MenuItems)
                      .WithOne(m => m.Restaurant)
                      .HasForeignKey(m => m.RestaurantId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // MenuItem configuration
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasPrecision(10, 2);
                entity.Property(e => e.Category).HasMaxLength(50);
            });
        }
    }
}
