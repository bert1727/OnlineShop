using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;

namespace OnlineShop.Context;

public class ProductDbContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ShoppingCart> Carts { get; set; } = null!;
    public DbSet<ShoppingCartProduct> CartProducts { get; set; } = null!;

    public ProductDbContext(DbContextOptions options)
        : base(options)
    {
        /* Database.EnsureDeleted(); */
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1 to 1      user to ShoppingCart
        modelBuilder
            .Entity<User>()
            .HasOne(static x => x.ShoppingCart)
            .WithOne(static x => x.User)
            .HasForeignKey<ShoppingCart>(static x => x.UserId);

        // 1 to many      product can have many associated ShoppingCartProduct
        modelBuilder
            .Entity<Product>()
            .HasMany(static x => x.ShoppingCartProducts)
            .WithOne(static x => x.Product)
            .HasForeignKey(static x => x.ProductId)
            .IsRequired(false);

        // 1 to many   shoppingCart can have many associated ShoppingCartProduct
        modelBuilder
            .Entity<ShoppingCart>()
            .HasMany(static x => x.ShoppingCartProducts)
            .WithOne(static x => x.ShoppingCart)
            .HasForeignKey(static x => x.ShoppingCartId);

        // Продукт
        modelBuilder
            .Entity<Product>()
            .HasData(
                [
                    new Product
                    {
                        Id = 1,
                        Name = "Laptop",
                        Price = 1500,
                        Category = "Electronics",
                        Description = "High-performance laptop",
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Tv",
                        Price = 1000,
                        Category = "Electronics",
                        Description = "High-performance laptop",
                    },
                ]
            );

        // Пользователь
        modelBuilder
            .Entity<User>()
            .HasData(
                [
                    new User
                    {
                        Id = 1,
                        Name = "John Doe",
                        Role = "Customer",
                        Password = "qwerty123",
                        Email = "johndoe@example.com",
                    },
                    new User
                    {
                        Id = 2,
                        Name = "Jack",
                        Role = "Customer",
                        Password = "qwerty123",
                        Email = "Jack@x.com",
                    },
                ]
            );

        // Корзина покупок
        modelBuilder
            .Entity<ShoppingCart>()
            .HasData(
                [
                    new ShoppingCart
                    {
                        Id = 1,
                        TotalPrice = 1500m,
                        Quantity = 1,
                        UserId = 1,
                    },
                    new ShoppingCart
                    {
                        Id = 2,
                        TotalPrice = 1000m,
                        Quantity = 1,
                        UserId = 2,
                    },
                ]
            );

        // Продукт в корзине покупок
        modelBuilder
            .Entity<ShoppingCartProduct>()
            .HasData(
                [
                    new ShoppingCartProduct
                    {
                        Id = 1,
                        Quantity = 1,
                        ProductId = 1,
                        ShoppingCartId = 1,
                    },
                    new ShoppingCartProduct
                    {
                        Id = 2,
                        Quantity = 100,
                        ProductId = 2,
                        ShoppingCartId = 2,
                    },
                ]
            );
    }
}
