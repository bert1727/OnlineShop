using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models.DTOs;
using OnlineShop.Services.Interfaces;

namespace OnlineShop.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, IUserService userService)
    {
        bool isDeleted = await userService.DeleteUser(id);
        return isDeleted ? NoContent() : NotFound();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, UserDto user)
    {
        if (id != user.Id)
            return BadRequest();
        bool isUpdated = await _userService.UpdateUser(id, user);
        return isUpdated ? NoContent() : NotFound();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var user = await _userService.GetUserById(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult> Post(UserCreationDto userDto)
    {
        var userNew = await _userService.AddUser(userDto);
        return Ok(userNew);
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var users = await _userService.GetUsers();
        return Ok(users);
    }
}
