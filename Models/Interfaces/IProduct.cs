namespace OnlineShop.Models.Interfaces;

public interface IProduct
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
}
