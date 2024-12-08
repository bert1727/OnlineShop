using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models.DTOs;

public class ProductDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public int Price { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Length must be between {2} and {1}")]
    public string Category { get; set; } = ""; // NOTE: сделать enum для категорий продуктов

    [Required]
    [StringLength(1000, MinimumLength = 3, ErrorMessage = "Length must be between {2} and {1}")]
    public string Description { get; set; } = "";
}
