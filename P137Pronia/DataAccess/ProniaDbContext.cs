using Microsoft.EntityFrameworkCore;
using P137Pronia.Models;

namespace P137Pronia.DataAccess
{
    public class ProniaDbContext:DbContext
    {
        public ProniaDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasIndex(p => p.Name).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
