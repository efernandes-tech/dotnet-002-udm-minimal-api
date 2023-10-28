using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<Product>? Products { get; set; }
    public DbSet<Category>? Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasKey(c => c.Id);
        modelBuilder.Entity<Category>().Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<Category>().Property(c => c.Description)
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder.Entity<Product>().HasKey(c => c.Id);
        modelBuilder.Entity<Product>().Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<Product>().Property(c => c.Description)
            .HasMaxLength(255);
        modelBuilder.Entity<Product>().Property(c => c.Image)
            .HasMaxLength(100);
        modelBuilder.Entity<Product>().Property(c => c.Price)
            .HasPrecision(14, 2);

        modelBuilder.Entity<Product>()
            .HasOne<Category>(p => p.Category)
                .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId);
    }
}
