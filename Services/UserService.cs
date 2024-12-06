using Microsoft.EntityFrameworkCore;
using OnlineShop.Context;
using OnlineShop.Models;
using OnlineShop.Models.DTOs;
using Onlineshop.Models.Enums;
using OnlineShop.Services.Interfaces;
using OnlineShop.Utilities;

namespace OnlineShop.Services;

public class UserService(OnlineShopDbContext context) : IUserService
{
    private readonly OnlineShopDbContext _context = context;

    public async Task<UserDto?> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        return user != null ? UserDtoUtils.UserToDto(user) : null;
    }

    public async Task<List<UserDto>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        return users.Select(UserDtoUtils.UserToDto).ToList();
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
            Role = Role.Guest,
        };

        await _context.Users.AddAsync(userNew);
        Console.WriteLine(userNew.Id);
        userNew.ShoppingCart = new ShoppingCart { UserId = userNew.Id };
        Console.WriteLine("User was addded");
        await _context.SaveChangesAsync();
        Console.WriteLine("User was saved");

        return UserDtoUtils.UserToDto(userNew);
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == id);
        if (user is null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
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
        }
        catch (DbUpdateConcurrencyException) when (_context.Users.Any(x => x.Id == id))
        {
            return false;
        }

        return true;
    }
}
