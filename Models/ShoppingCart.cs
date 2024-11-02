namespace OnlineShop.Models;

public class ShoppingCart
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public int Quantity { get; set; }

    public User User { get; set; } = null!;
    public int UserId { get; set; }

    public List<ShoppingCartProduct> ShoppingCartProducts { get; set; } = [];
}
