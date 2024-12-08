using Microsoft.EntityFrameworkCore;
using OnlineShop.Context;
using OnlineShop.Models;
using OnlineShop.Models.DTOs;
using Onlineshop.Models.Enums;
using OnlineShop.Services.Interfaces;
using OnlineShop.Utilities;
using Serilog;

namespace OnlineShop.Services;

public class UserService(ILogger<UserService> logger, OnlineShopDbContext context) : IUserService
{
    private readonly OnlineShopDbContext _context = context;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        return user;
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
            Role = Role.Customer,
            ShoppingCart = new ShoppingCart(),
        };
        /* var shoppingCartnew = new ShoppingCart { }; */
        /* userNew.ShoppingCart = shoppingCartnew; */
        /* userNew.ShoppingCartId = shoppingCartnew.Id; */
        await _context.Users.AddAsync(userNew);
        var userDto = UserDtoUtils.UserToDto(userNew);
        Console.WriteLine($"Lalalla: {userDto}");
        Log.Information("User was added {@user}", userDto);
        /* _logger.LogInformation("Ultra user {User}", userDto); */
        await _context.SaveChangesAsync();
        Log.Information("User was saved {@user}", userDto);

        /* userNew.ShoppingCart.UserId = userNew.Id; */
        /* await _context.SaveChangesAsync(); */
        /* var shoppingCartNew = new ShoppingCart { UserId = userNew.Id }; */
        /* userNew.ShoppingCart = shoppingCartNew; */
        /* userNew.ShoppingCartId = shoppingCartNew.Id; */
        /* ShoppingCart = new ShoppingCart { UserId = userNew.Id } */

        Console.WriteLine(userNew.Id);

        /* userNew.ShoppingCartId = shoppingCartNew.Id; */
        /* userNew.ShoppingCartId = shoppingCartNew.Id; */
        /* _logger.LogInformation("User was added"); */
        /* await _context.Carts.AddAsync(shoppingCartNew); */
        /* await _context.SaveChangesAsync(); */
        /* _logger.LogInformation($"{userNew.Id}"); */
        /* _logger.LogInformation("User was saved"); */

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
