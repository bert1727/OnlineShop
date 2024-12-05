using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models.DTOs;
using OnlineShop.Services.Interfaces;
using OnlineShop.Utilities.Logging;

namespace OnlineShop.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProductController(ILogger<ProductController> logger, IProductService productService)
    : ControllerBase
{
    private readonly ILogger _logger = logger;
    private readonly IProductService _productService = productService;

    /// <summary>
    /// Получает список всех продуктов.
    /// </summary>
    /// <returns>Список продуктов.</returns>
    /// <response code="200">Возвращает все продукты существующие</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        _logger.LogInfoProductController("Get is running");
        var products = await _productService.GetProducts();
        return Ok(products);
    }

    /// <summary>
    /// Получить продукт по id
    /// </summary>
    /// <returns>Продукт с данным id</returns>
    /// <response code="200">Возвращает продукт.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        _logger.LogInfoProductController("GetById is running");
        var product = await _productService.GetProductById(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        bool isdDeleted = await _productService.DeleteProduct(id);
        return isdDeleted ? NoContent() : NotFound();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, ProductDto product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }
        bool isUpdated = await _productService.UpdateProduct(id, product);
        return isUpdated ? NoContent() : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> Post(ProductDto product)
    {
        var createdProduct = await _productService.CreateProduct(product);
        return Ok(createdProduct);
    }

    // NOTE: just test an Authorization
    [HttpGet("getallauth")]
    [Authorize]
    public ActionResult GetAllProducts()
    {
        Console.WriteLine("GetAllProducts is running");
        string[] a = ["Product1", "Product2"];
        return Ok(a);
    }

    [HttpPost("postauth")]
    [Authorize]
    public ActionResult AddProduct([FromBody] string product)
    {
        Console.WriteLine("Postauth is running");
        return Ok($"Product {product} added");
    }
}
