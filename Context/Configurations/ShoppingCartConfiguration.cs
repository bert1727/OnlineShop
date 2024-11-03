using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Models;

namespace OnlineShop.Context.Configurations;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        // 1 to many   shoppingCart can have many associated ShoppingCartProduct
        builder
            .HasMany(static x => x.ShoppingCartProducts)
            .WithOne(static x => x.ShoppingCart)
            .HasForeignKey(static x => x.ShoppingCartId);

        // Корзина покупок
        builder.HasData(
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
    }
}
