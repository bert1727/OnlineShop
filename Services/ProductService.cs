using Microsoft.EntityFrameworkCore;
using OnlineShop.Context;
using OnlineShop.Models;
using OnlineShop.Models.DTOs;
using OnlineShop.Services.Interfaces;
using OnlineShop.Utilities;
using Serilog;

namespace OnlineShop.Services;

public class ProductService(OnlineShopDbContext context) : IProductService
{
    private readonly OnlineShopDbContext _context = context;

    public async Task<List<ProductDto>> GetProducts()
    {
        var products = await _context.Products.ToListAsync();

        var productsDto = products.Select(ProductDtoUtils.ProductToDto).ToList();

        Log.Information("All products: {@products}", productsDto);

        return productsDto;
    }

    public async Task<ProductDto?> GetProductById(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        Log.Information("Product with Id: {@id} was found", id);

        return product is null ? null : ProductDtoUtils.ProductToDto(product);
    }

    public async Task<ProductDto> CreateProduct(ProductDto product)
    {
        var productNew = new Product
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
        };

        await _context.Products.AddAsync(productNew);
        await _context.SaveChangesAsync();

        var productDto = ProductDtoUtils.ProductToDto(productNew);

        Log.Information("Product was added and saved {@Product}", productDto);

        return productDto;
    }

    // NOTE: возможно тут в отдельную функцию вынести проверку на существование объекта, ну или нет
    // NOTE: Здесь как вариант можно просто возвращать IActionResult
    public async Task<bool> DeleteProduct(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateProduct(int id, ProductDto product)
    {
        var productUpdate = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (productUpdate is null)
            return false;

        product.Id = product.Id;
        productUpdate.Name = product.Name;
        productUpdate.Price = product.Price;
        productUpdate.Category = product.Category;
        productUpdate.Description = product.Description;

        try
        {
            await _context.SaveChangesAsync();
            Log.Information("Product with Id: {@id} was updated", id);
        }
        catch (DbUpdateConcurrencyException e) when (_context.Products.Any(x => x.Id == id))
        {
            Log.Information("Failed to update product with Id: {@id}, error: {@err}", id, e);
            return false;
        }

        return true;
    }
    /* private bool TodoItemExists(int id) */
    /* { */
    /*     return _context.ToDoItems.Any(x => x.Id == id); */
    /* } */
}
