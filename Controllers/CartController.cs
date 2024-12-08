using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models.DTOs;
using OnlineShop.Services.Interfaces;

namespace OnlineShop.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController(ICartService cartService) : ControllerBase
{
    private readonly ICartService _cartService = cartService;

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        return Ok(await _cartService.GetCartProducts(id));
    }

    [HttpPost]
    public async Task<ActionResult> Post(CartProductForm cartProduct)
    {
        return await _cartService.AddProductToCart(cartProduct) ? Ok() : NotFound();
    }

    [HttpPut]
    public async Task<ActionResult> Put(CartProductForm cartProduct)
    {
        return await _cartService.UpdateProductInCart(cartProduct) ? Ok() : NotFound();
    }

    [HttpDelete]
    // NOTE: or use query params here instead of CartProductDeleteFormDto
    public async Task<ActionResult> Delete(CartProductDeleteFormDto cartProduct)
    {
        return await _cartService.RemoveProductFromCart(cartProduct) ? Ok() : NotFound();
    }
}
