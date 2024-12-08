using Microsoft.EntityFrameworkCore;
using OnlineShop.Context;
using OnlineShop.Models;
using OnlineShop.Models.DTOs;
using Onlineshop.Models.Enums;
using OnlineShop.Services.Interfaces;
using OnlineShop.Utilities;
using Serilog;

namespace OnlineShop.Services;

public class UserService(OnlineShopDbContext context) : IUserService
{
    private readonly OnlineShopDbContext _context = context;

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        return user;
    }

    public async Task<List<UserDto>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        var usersDto = users.Select(UserDtoUtils.UserToDto).ToList();

        Log.Information("All users: {@users}", usersDto);

        return usersDto;
    }

    public async Task<UserDto?> GetUserById(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        return user is null ? null : UserDtoUtils.UserToDto(user);
    }

    public async Task<UserDto> AddUser(UserCreationDto user) // NOTE: либо тут только id Dto возвращать
    {
        var userNew = new User
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Role = Role.Customer,
            ShoppingCart = new ShoppingCart(),
        };

        await _context.Users.AddAsync(userNew);
        await _context.SaveChangesAsync();

        var userDto = UserDtoUtils.UserToDto(userNew);

        Log.Information("User was added and saved {@User}", userDto);

        return userDto;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == id);

        if (user is null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        Log.Information("User with Id: {@id} was deleted", id);

        return true;
    }

    public async Task<bool> UpdateUser(int id, UserDto user)
    {
        var userUpdate = _context.Users.FirstOrDefault(x => x.Id == id);
        if (userUpdate is null)
        {
            return false;
        }

        userUpdate.Name = user.Name;
        userUpdate.Email = user.Email;
        userUpdate.Password = user.Password;
        userUpdate.Role = user.Role;

        try
        {
            await _context.SaveChangesAsync();
            Log.Information("User with Id: {@id} was updated", id);
        }
        catch (DbUpdateConcurrencyException e) when (_context.Users.Any(x => x.Id == id))
        {
            Log.Error("Failed to update user with Id: {@id}, error: {@e}", id, e);
            return false;
        }

        return true;
    }
}
