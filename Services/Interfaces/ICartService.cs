using OnlineShop.Models.DTOs;

namespace OnlineShop.Services.Interfaces;

public interface ICartService
{
    Task<List<ProductDto>> GetCartProducts(int userId);
    Task<bool> AddProductToCart(CartProductForm cartProduct);
    Task<bool> UpdateProductInCart(CartProductForm cartProduct);
    Task<bool> RemoveProductFromCart(CartProductDeleteFormDto cartProduct);
}
