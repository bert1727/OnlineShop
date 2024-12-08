using System.Text.Json.Serialization;
using Onlineshop.Models.Enums;

namespace OnlineShop.Models.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Role Role { get; set; }
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}
