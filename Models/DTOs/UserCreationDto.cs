using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models.DTOs;

public class UserCreationDto
{
    [Required]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = "";

    [DataType(DataType.Password)]
    [StringLength(
        100,
        MinimumLength = 8,
        ErrorMessage = "Field {0} must have at least {2} and max {1} symbols."
    )]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Please confirm your password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = "";
}
