using Microsoft.AspNetCore.Http.HttpResults;
using OnlineShop.Models.DTOs;
using OnlineShop.Services.Interfaces;

namespace OnlineShop.ControllersMinimal;

public static class UserControllerMinimal
{
    public static void MapUserController(this WebApplication app)
    {
        var endpoints = app.MapGroup("/api/users").WithOpenApi();

        endpoints.MapGet("/", Get).WithSummary("Get all users");
        endpoints.MapGet("/{id}", GetById).WithSummary("Get user by id");
        endpoints.MapPost("/", Post).WithSummary("Add user");
        endpoints.MapPut("/{id}", Put).WithSummary("Update user");
        endpoints.MapDelete("/{id}", Delete).WithSummary("Delete user");
    }

    private static async Task<Results<NoContent, NotFound>> Delete(int id, IUserService userService)
    {
        bool isDeleted = await userService.DeleteUser(id);
        return isDeleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Results<NoContent, NotFound, BadRequest>> Put(
        int id,
        UserDto user,
        IUserService userService
    )
    {
        if (id != user.Id)
            return TypedResults.BadRequest();
        bool isUpdated = await userService.UpdateUser(id, user);
        return isUpdated ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<UserDto>, NotFound>> GetById(
        int id,
        IUserService userService
    )
    {
        var user = await userService.GetUserById(id);
        return user is null ? TypedResults.NotFound() : TypedResults.Ok(user);
    }

    private static async Task<Ok<UserDto>> Post(UserDto userDto, IUserService userService)
    {
        var userNew = await userService.AddUser(userDto);
        return TypedResults.Ok(userNew);
    }

    private static async Task<Ok<List<UserDto>>> Get(IUserService userService)
    {
        var users = await userService.GetUsers();
        return TypedResults.Ok(users);
    }
}
