using Onlineshop.Models.Enums;

namespace OnlineShop.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public Role Role { get; set; }
    public string Password { get; set; } = ""; // NOTE: его надо захэшировать и поля обязательные долджны быть
    public string Email { get; set; } = "";

    // Foreign key
    public ShoppingCart ShoppingCart { get; set; } = null!;
}
