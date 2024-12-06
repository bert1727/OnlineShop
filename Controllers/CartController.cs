using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Services.Interfaces;

namespace OnlineShop.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController(ILogger<ProductController> logger, ICartService cartService)
    : ControllerBase
{
    // TODO: use logger
    private readonly ILogger _logger = logger;
    private readonly ICartService _cartService = cartService;

    [HttpDelete("{userId:int}/{productId:int}")]
    public async Task<ActionResult> Delete(int userId, int productId)
    {
        return await _cartService.RemoveProductFromCart(productId, userId) ? Ok() : NotFound();
    }

    // FIXME: create a normal dto model for it
    [HttpPut]
    public async Task<ActionResult> Put([FromBody] RequestForm request)
    {
        return await _cartService.UpdateProductInCart(
            request.ProductId,
            request.UserId,
            request.Quantity
        )
            ? Ok()
            : NotFound();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        return Ok(await _cartService.GetCartProducts(id));
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] RequestForm request, ICartService cartService)
    {
        return await cartService.AddProductToCart(
            request.ProductId,
            request.UserId,
            request.Quantity
        )
            ? Ok()
            : NotFound();
    }
}
