namespace OnlineShop.Models.DTOs;

public class CartProductForm
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CartProductDeleteFormDto
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
}
