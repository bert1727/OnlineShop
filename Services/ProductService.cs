using Microsoft.EntityFrameworkCore;
using OnlineShop.Context;
using OnlineShop.Models;
using OnlineShop.Models.DTOs;
using OnlineShop.Services.Interfaces;

namespace OnlineShop.Services;

public class ProductService(ProductDbContext context) : IProductService
{
    private readonly ProductDbContext _context = context;

    public async Task<List<Product>> GetProducts()
    {
        var products = await _context.Products.ToListAsync();
        return products ?? [];
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Product> CreateProduct(ProductDto product)
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
        return productNew;
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
        }
        catch (DbUpdateConcurrencyException) when (_context.Products.Any(x => x.Id == id))
        {
            return false;
        }

        return true;
    }
    /* private bool TodoItemExists(int id) */
    /* { */
    /*     return _context.ToDoItems.Any(x => x.Id == id); */
    /* } */
}
