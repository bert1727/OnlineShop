using Microsoft.EntityFrameworkCore;
using OnlineShop.Context;
using OnlineShop.Models;
using OnlineShop.Models.DTOs;
using OnlineShop.Services.Interfaces;
using OnlineShop.Utilities;
using Serilog;

namespace OnlineShop.Services;

public class CartService(OnlineShopDbContext context) : ICartService
{
    private readonly OnlineShopDbContext _context = context;

    // TODO: change return type
    public async Task<bool> AddProductToCart(int productId, int userId, int quantity)
    {
        var ShoppingCartProducts = await _context.Carts.FirstOrDefaultAsync(x =>
            x.UserId == userId
        );

        if (ShoppingCartProducts == null)
            return false;
        /* return "User or shopping cart not found"; */
        Log.Information("Cart was found");

        var product = await _context.Products.FindAsync(productId);

        if (product == null)
            return false;
        /* return "Product not found"; */
        Log.Information("Product was found");

        var existingProductInCart = ShoppingCartProducts.ShoppingCartProducts.FirstOrDefault(x =>
            x.ProductId == productId
        );

        if (existingProductInCart != null)
        {
            existingProductInCart.Quantity += quantity;

            await _context.SaveChangesAsync();

            Log.Information("Quantity product in cart updated");

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
            Log.Information("New product added to cart");
            return true;
        }
    }

    public async Task<List<ProductDto>> GetCartProducts(int userId)
    {
        var allProductsInCart = await _context
            .CartProducts.Where(x => x.ShoppingCartId == userId)
            .ToListAsync();

        Log.Information("Count of products in cart {@count}", allProductsInCart.Count);

        var productsInCart = allProductsInCart.Select(x => x.ProductId).ToList();

        Log.Information(
            "Cart was found and products in cart was found, count: {@count}",
            productsInCart.Count
        );

        var res = new List<ProductDto>();
        foreach (int? item in productsInCart)
        {
            if (item == null)
                continue;

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == item);

            // there is no null product probably
            if (product == null)
                continue;

            res.Add(ProductDtoUtils.ProductToDto(product));
        }

        Log.Information("Products in cart are {@res}", res);
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
            Log.Information("Product not found");
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

        Log.Information("Product was deleted");

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
        Log.Information("User was found");

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            return false;
        /* return "Product not found"; */
        Log.Information("Product was found");

        var existingProductInCart = user.ShoppingCart.ShoppingCartProducts.FirstOrDefault(x =>
            x.ProductId == productId
        );

        if (existingProductInCart == null)
        {
            Log.Information("No product in cart");
            return false;
            /* return "Prouduct doesn't exist in cart"; */
        }

        existingProductInCart.Quantity += quantity;

        await _context.SaveChangesAsync();

        Log.Information(
            "Quantity of product in cart updated: {@quantity}",
            existingProductInCart.Quantity
        );

        return true;
        /* return "Product updated in cart"; */
    }
}
