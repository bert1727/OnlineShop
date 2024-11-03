using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Models;

namespace OnlineShop.Context.Configurations;

public class ShoppingCartProductConfiguration : IEntityTypeConfiguration<ShoppingCartProduct>
{
    public void Configure(EntityTypeBuilder<ShoppingCartProduct> builder)
    {
        builder.HasData(
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
