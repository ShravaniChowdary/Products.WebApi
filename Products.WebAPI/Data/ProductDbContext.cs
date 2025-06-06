using Microsoft.EntityFrameworkCore;
using Products.WebAPI.Models.Domain;

namespace Products.Api.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Id)
                .UseIdentityColumn(seed: 100000, increment: 1);

                entity.Property(p => p.Price)
                .HasPrecision(18, 3);
            });
                
        }
    }
}
