using OnlineShop.Models.DTOs;

namespace OnlineShop.Services.Interfaces;

public interface IUserService
{
    public Task<List<UserDto>> GetUsers();
    public Task<UserDto?> GetUserById(int id);
    public Task<UserDto> AddUser(UserDto user);
    public Task<bool> DeleteUser(int id);
    public Task<bool> UpdateUser(int id, UserDto user);
}
