using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Models.DTOs;
using OnlineShop.Services.Interfaces;

namespace OnlineShop.Controllers;

public static class CartController
{
    public static void MapCartController(this WebApplication app)
    {
        var endpoints = app.MapGroup("/api/cart").WithOpenApi();

        endpoints.MapGet("/{id}", Get).WithSummary("Get all products in cart");
        endpoints.MapPost("/", Post).WithSummary("Add product to cart");
        endpoints.MapPut("/", Put).WithSummary("Update product");
        endpoints.MapDelete("/{userId}/{productId}", Delete).WithSummary("Delete product");
    }

    private static async Task<List<ProductDto>> Get(int id, ICartService cartService)
    {
        return await cartService.GetCartProducts(id);
    }

    private static async Task<Results<Ok, NotFound>> Put(
        [FromBody] RequestForm request,
        ICartService cartService
    )
    {
        return await cartService.UpdateProductInCart(
            request.ProductId,
            request.UserId,
            request.Quantity
        )
            ? TypedResults.Ok()
            : TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound>> Post(
        [FromBody] RequestForm request,
        ICartService cartService
    )
    {
        return await cartService.AddProductToCart(
            request.ProductId,
            request.UserId,
            request.Quantity
        )
            ? TypedResults.Ok()
            : TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound>> Delete(
        int userId,
        int productId,
        ICartService cartService
    )
    {
        return await cartService.RemoveProductFromCart(productId, userId)
            ? TypedResults.Ok()
            : TypedResults.NotFound();
    }
}
