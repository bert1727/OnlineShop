using Microsoft.EntityFrameworkCore;
using OnlineShop.Context;
using OnlineShop.Models;
using OnlineShop.Services.Interfaces;

namespace OnlineShop.Services;

public class CartService(ProductDbContext context) : ICartService
{
    private readonly ProductDbContext _context = context;

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
            /* return "Product added to cart"; */
        }
    }

    public async Task<List<ShoppingCartProduct>> GetCartProducts(int userId)
    {
        /* var user = await _context */
        /*     .Users.Include(static u => u.ShoppingCart) */
        /*     .ThenInclude(static x => x.ShoppingCartProducts) */
        /*     .ThenInclude(static x => x.Product) */
        /*     .FirstOrDefaultAsync(u => u.Id == userId); */
        var shoppingCart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
        if (shoppingCart == null)
        {
            Console.WriteLine("Cart is null");
            return [];
        }
        var userProducts = shoppingCart.ShoppingCartProducts;
        if (userProducts == null)
        {
            Console.WriteLine("userProducts is null");
        }
        return userProducts ?? [];
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
