namespace OnlineShop.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Price { get; set; }
    public string Category { get; set; } = ""; // NOTE: сделать enum для категорий продуктов
    public string Description { get; set; } = "";

    public List<ShoppingCartProduct> ShoppingCartProducts { get; set; } = [];
    /* public List<ShoppingCart> ShoppingCart { get; set; } = []; */
}
