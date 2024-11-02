namespace OnlineShop.Models.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Price { get; set; }
    public string Category { get; set; } = ""; // NOTE: сделать enum для категорий продуктов
    public string Description { get; set; } = "";
}
