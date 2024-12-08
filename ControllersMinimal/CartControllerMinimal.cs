using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models.DTOs;
using OnlineShop.Services.Interfaces;

namespace OnlineShop.ControllersMinimal;

public static class CartControllerMinimal
{
    public static void MapCartController(this WebApplication app)
    {
        var endpoints = app.MapGroup("/api/cart").WithOpenApi();

        endpoints.MapGet("/{id}", GetById).WithSummary("Get all products in cart");
        endpoints.MapPost("/", Post).WithSummary("Add product to cart");
        endpoints.MapPut("/", Put).WithSummary("Update product");
        endpoints.MapDelete("/", Delete).WithSummary("Delete product");
    }

    private static async Task<List<ProductDto>> GetById(int id, ICartService cartService)
    {
        return await cartService.GetCartProducts(id);
    }

    private static async Task<Results<Ok, NotFound>> Post(
        CartProductForm cartProduct,
        ICartService cartService
    )
    {
        return await cartService.AddProductToCart(cartProduct)
            ? TypedResults.Ok()
            : TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound>> Put(
        CartProductForm cartProduct,
        ICartService cartService
    )
    {
        return await cartService.UpdateProductInCart(cartProduct)
            ? TypedResults.Ok()
            : TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound>> Delete(
        [FromBody] CartProductDeleteFormDto cartDeleteProduct,
        ICartService cartService
    )
    {
        return await cartService.RemoveProductFromCart(cartDeleteProduct)
            ? TypedResults.Ok()
            : TypedResults.NotFound();
    }
}
