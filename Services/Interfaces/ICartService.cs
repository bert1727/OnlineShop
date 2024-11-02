using OnlineShop.Models.DTOs;

namespace OnlineShop.Services.Interfaces;

public interface ICartService
{
    Task<List<ProductDto>> GetCartProducts(int userId);
    Task<bool> AddProductToCart(int productId, int userId, int quantity);
    Task<bool> UpdateProductInCart(int productId, int userId, int quantity);
    Task<bool> RemoveProductFromCart(int productId, int userId);
}
