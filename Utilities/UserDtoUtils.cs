using OnlineShop.Models;
using OnlineShop.Models.DTOs;

namespace OnlineShop.Utilities;

public static class UserDtoUtils
{
    public static UserDto UserToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role,
            Email = user.Email,
            Password = user.Password,
        };
    }
}
