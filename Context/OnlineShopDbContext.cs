using Microsoft.EntityFrameworkCore;
using OnlineShop.Context.Configurations;
using OnlineShop.Models;

namespace OnlineShop.Context;

public class OnlineShopDbContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ShoppingCart> Carts { get; set; } = null!;
    public DbSet<ShoppingCartProduct> CartProducts { get; set; } = null!;

    public OnlineShopDbContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ShoppingCartConfiguration());
        modelBuilder.ApplyConfiguration(new ShoppingCartProductConfiguration());
    }
}
