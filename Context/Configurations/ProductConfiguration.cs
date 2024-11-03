using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Models;

namespace OnlineShop.Context.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // 1 to many      product can have many associated ShoppingCartProduct
        builder
            .HasMany(static x => x.ShoppingCartProducts)
            .WithOne(static x => x.Product)
            .HasForeignKey(static x => x.ProductId)
            .IsRequired(false);

        // Продукт
        builder.HasData(
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
    }
}
