using OnlineShop.Models;
using OnlineShop.Models.DTOs;

namespace OnlineShop.Services.Interfaces;

public interface IProductService
{
    public Task<List<Product>> GetProducts();
    public Task<Product?> GetProductById(int id);
    public Task<Product> CreateProduct(ProductDto product);
    public Task<bool> DeleteProduct(int id);
    public Task<bool> UpdateProduct(int id, ProductDto product);
}
