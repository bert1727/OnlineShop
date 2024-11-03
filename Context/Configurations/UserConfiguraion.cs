using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Models;
using Onlineshop.Models.Enums;

namespace OnlineShop.Context.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // 1 to 1      user to ShoppingCart
        builder
            .HasOne(static x => x.ShoppingCart)
            .WithOne(static x => x.User)
            .HasForeignKey<ShoppingCart>(static x => x.UserId);

        builder.Property(static x => x.Role).HasConversion<string>();

        builder.HasData(
            [
                new User
                {
                    Id = 1,
                    Name = "John Doe",
                    Role = Role.Customer,
                    Password = "qwerty123",
                    Email = "johndoe@example.com",
                },
                new User
                {
                    Id = 2,
                    Name = "Jack",
                    Role = Role.Customer,
                    Password = "qwerty123",
                    Email = "Jack@x.com",
                },
            ]
        );
    }
}
