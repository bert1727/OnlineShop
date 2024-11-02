namespace OnlineShop.Models;

public class ShoppingCartProduct
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int? ProductId { get; set; }
    public Product? Product { get; set; } = null!;

    public int ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; } = null!;
}
