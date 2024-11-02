using OnlineShop.Models.DTOs;

namespace OnlineShop.Services.Interfaces;

public interface IProductService
{
    public Task<List<ProductDto>> GetProducts();
    public Task<ProductDto?> GetProductById(int id);
    public Task<ProductDto> CreateProduct(ProductDto product);
    public Task<bool> DeleteProduct(int id);
    public Task<bool> UpdateProduct(int id, ProductDto product);
}
