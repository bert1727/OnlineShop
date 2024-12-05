using Microsoft.AspNetCore.Http.HttpResults;
using OnlineShop.Models.DTOs;
using OnlineShop.Services.Interfaces;

/* using Microsoft.AspNetCore.Mvc; */
/* using Microsoft.EntityFrameworkCore; */

namespace OnlineShop.ControllersMinimal;

public static class ProductControllerMinimal
{
    public static void MapProductController(this WebApplication app)
    {
        var endpoints = app.MapGroup("/api/products").WithOpenApi();

        endpoints.MapGet("/", Get).WithSummary("Get all products");
        endpoints.MapGet("/{id}", GetById).WithSummary("Get product by id");
        endpoints.MapPost("/", Post).WithSummary("Add product");
        endpoints.MapPut("/{id}", Put).WithSummary("Update product");
        endpoints.MapDelete("/{id}", Delete).WithSummary("Delete product");
    }

    private static async Task<Results<Ok<List<ProductDto>>, NotFound>> Get(
        IProductService productService
    )
    {
        var products = await productService.GetProducts();
        return TypedResults.Ok(products);
    }

    private static async Task<Results<NoContent, NotFound>> Delete(
        int id,
        IProductService productService
    )
    {
        bool isdDeleted = await productService.DeleteProduct(id);
        return isdDeleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Results<NoContent, NotFound, BadRequest>> Put(
        int id,
        ProductDto product,
        IProductService productService
    )
    {
        if (id != product.Id)
        {
            return TypedResults.BadRequest();
        }
        bool isUpdated = await productService.UpdateProduct(id, product);
        return isUpdated ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<ProductDto>, NotFound>> GetById(
        int id,
        IProductService productService
    )
    {
        var product = await productService.GetProductById(id);
        return product is null ? TypedResults.NotFound() : TypedResults.Ok(product);
    }

    private static async Task<Ok<ProductDto>> Post(
        ProductDto product,
        IProductService productService
    )
    {
        var createdProduct = await productService.CreateProduct(product);
        return TypedResults.Ok(createdProduct);
    }
}
