using OnlineShop.Models;
using OnlineShop.Models.DTOs;

namespace OnlineShop.Utilities;

public static class ProductDtoUtils
{
    public static ProductDto ProductToDto(Product product)
    {
        Console.WriteLine($"Id is:{product.Id}", product.Id);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
        };
    }
}
