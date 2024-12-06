using Microsoft.EntityFrameworkCore;
using OnlineShop.Context;
using OnlineShop.Models;
using OnlineShop.Models.DTOs;
using OnlineShop.Services.Interfaces;
using OnlineShop.Utilities;

namespace OnlineShop.Services;

public class CartService(OnlineShopDbContext context) : ICartService
{
    private readonly OnlineShopDbContext _context = context;

    public async Task<bool> AddProductToCart(int productId, int userId, int quantity)
    {
        var ShoppingCartProducts = await _context.Carts.FirstOrDefaultAsync(x =>
            x.UserId == userId
        );

        if (ShoppingCartProducts == null)
            return false;
        /* return "User or shopping cart not found"; */

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            return false;
        /* return "Product not found"; */

        var existingProductInCart = ShoppingCartProducts.ShoppingCartProducts.FirstOrDefault(x =>
            x.ProductId == productId
        );

        if (existingProductInCart != null)
        {
            existingProductInCart.Quantity += quantity;
            await _context.SaveChangesAsync();
            return true;
            /* return "Product updated in cart"; */
        }
        else
        {
            ShoppingCartProducts.ShoppingCartProducts.Add(
                new ShoppingCartProduct
                {
                    Product = product,
                    ProductId = productId,
                    Quantity = quantity,
                }
            );
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public async Task<List<ProductDto>> GetCartProducts(int userId)
    {
        var a = await _context.CartProducts.Where(x => x.ShoppingCartId == userId).ToListAsync();

        Console.WriteLine($"Test {a}, size of a: {a.Count}");
        var b = a.Select(x => x.ProductId).ToList();
        Console.WriteLine($"Cart was found and products was found, size: {b.Count}");
        var res = new List<ProductDto>();
        foreach (int? item in b)
        {
            if (item == null)
                continue;
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == item);
            if (product == null)
                continue;
            res.Add(ProductDtoUtils.ProductToDto(product));
        }
        Console.WriteLine(res.ToString());
        return res;
    }

    public async Task<bool> RemoveProductFromCart(int productId, int userId)
    {
        var user = await _context
            .Users.Include(static u => u.ShoppingCart)
            .ThenInclude(static x => x.ShoppingCartProducts)
            .ThenInclude(static x => x.Product)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }
        var product = user.ShoppingCart.ShoppingCartProducts.FirstOrDefault(x =>
            x.ProductId == productId
        );
        if (product != null)
        {
            user.ShoppingCart.ShoppingCartProducts.Remove(product);
            await _context.SaveChangesAsync();
        }
        return true;
    }

    public async Task<bool> UpdateProductInCart(int productId, int userId, int quantity)
    {
        var user = await _context
            .Users.Include(static x => x.ShoppingCart)
            .ThenInclude(static x => x.ShoppingCartProducts)
            .ThenInclude(static x => x.Product)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null || user.ShoppingCart == null)
            return false;
        /* return "User or shopping cart not found"; */

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            return false;
        /* return "Product not found"; */

        var existingProductInCart = user.ShoppingCart.ShoppingCartProducts.FirstOrDefault(x =>
            x.ProductId == productId
        );

        if (existingProductInCart == null)
        {
            return false;
            /* return "Prouduct doesn't exist in cart"; */
        }
        existingProductInCart.Quantity += quantity;
        await _context.SaveChangesAsync();
        return true;
        /* return "Product updated in cart"; */
    }
}
