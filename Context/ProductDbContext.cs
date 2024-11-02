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
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Price = 1500,
                    Category = "Electronics", // Позже можно заменить на enum
                    Description = "High-performance laptop",
                }
            );

        // Пользователь
        modelBuilder
            .Entity<User>()
            .HasData(
                new User
                {
                    Id = 1,
                    Name = "John Doe",
                    Role = "Customer",
                    Password = "hashed_password", // Захешированный пароль
                    Email = "johndoe@example.com",
                }
            );

        // Корзина покупок
        modelBuilder
            .Entity<ShoppingCart>()
            .HasData(
                new ShoppingCart
                {
                    Id = 1,
                    TotalPrice = 1500m,
                    Quantity = 1,
                    UserId =
                        1 // Привязка к существующему пользователю
                    ,
                }
            );

        // Продукт в корзине покупок
        modelBuilder
            .Entity<ShoppingCartProduct>()
            .HasData(
                new ShoppingCartProduct
                {
                    Id = 1,
                    Quantity = 1,
                    ProductId = 1, // Привязка к существующему продукту
                    ShoppingCartId =
                        1 // Привязка к существующей корзине
                    ,
                }
            );

        // 1 to many   shoppingCart to ShoppingCartProduct
        /* modelBuilder */
        /*     .Entity<ShoppingCart>() */
        /*     .HasMany(static x => x.ShoppingCartProducts) */
        /*     .WithOne(static x => x.ShoppingCart) */
        /*     .HasForeignKey(static x => x.ShoppingCartId) */
        /*     .HasForeignKey(static x => x.ProductId); */

        /* modelBuilder */
        /*     .Entity<ShoppingCartProduct>() */
        /*     .HasKey(static x => new { x.ShoppingCartId, x.ProductId }); */

        /* modelBuilder */
        /*     .Entity<ShoppingCartProduct>() */
        /*     .HasOne(static x => x.ShoppingCart) */
        /*     .WithMany(static x => x.ShoppingCartProducts) */
        /*     .HasForeignKey(static x => x.ShoppingCartId); */
        /**/
        /* modelBuilder */
        /*     .Entity<ShoppingCartProduct>() */
        /*     .HasOne(static x => x.Product) */
        /*     .WithMany() */
        /*     .HasForeignKey(static x => x.ProductId); */

        // Seeding db with test data
        /* modelBuilder */
        /*     .Entity<User>() */
        /*     .HasData( */
        /*         new User */
        /*         { */
        /*             Id = 1, */
        /*             Name = "1", */
        /*             Email = "1@1", */
        /*             Password = "123", */
        /*             Role = "guest", */
        /*         } */
        /*     ); */
        /* modelBuilder */
        /*     .Entity<ShoppingCart>() */
        /*     .HasData( */
        /*         new ShoppingCart */
        /*         { */
        /*             Id = 1, */
        /*             TotalPrice = 0, */
        /*             Quantity = 0, */
        /*             UserId = 1, */
        /*         } */
        /*     ); */
        /* modelBuilder */
        /*     .Entity<Product>() */
        /*     .HasData( */
        /*         new Product */
        /*         { */
        /*             Id = 1, */
        /*             Name = "Test", */
        /*             Price = 100, */
        /*             Category = "test", */
        /*             Description = "Test", */
        /*         } */
        /*     ); */
        /* modelBuilder */
        /*     .Entity<Product>() */
        /*     .HasData( */
        /*         new Product */
        /*         { */
        /*             Id = 1, */
        /*             Name = "First product", */
        /*             Price = 100, */
        /*             Category = "Category.Food", */
        /*             Description = "Test", */
        /*         } */
        /*     ); */



        // Добавление тестового пользователя
        /* modelBuilder */
        /*     .Entity<User>() */
        /*     .HasData( */
        /*         new User */
        /*         { */
        /*             Id = 1, */
        /*             Name = "Test User", */
        /*             Role = "Customer", */
        /*             Password = "TestPassword", */
        /*             Email = "testuser@example.com", */
        /*         } */
        /*     ); */
        /**/
        /* // Добавление тестового продукта */
        /* modelBuilder */
        /*     .Entity<Product>() */
        /*     .HasData( */
        /*         new Product */
        /*         { */
        /*             Id = 1, */
        /*             Name = "Test Product", */
        /*             Price = 100, */
        /*             Category = "Electronics", */
        /*             Description = "A test product for the online shop.", */
        /*         } */
        /*     ); */
        /**/
        /* // Добавление тестовой корзины */
        /* modelBuilder */
        /*     .Entity<ShoppingCart>() */
        /*     .HasData( */
        /*         new ShoppingCart */
        /*         { */
        /*             Id = 1, */
        /*             TotalPrice = 0, */
        /*             UserId = 1, */
        /*             // Связываем корзину с тестовым пользователем */
        /*         } */
        /*     ); */
        /**/
        /* // Добавление тестового продукта в корзину */
        /* modelBuilder */
        /*     .Entity<ShoppingCartProduct>() */
        /*     .HasData( */
        /*         new ShoppingCartProduct */
        /*         { */
        /*             ShoppingCartId = 1, */
        /*             ProductId = 1, */
        /*             Quantity = 1, */
        /*         } */
        /*     ); */
    }
}
