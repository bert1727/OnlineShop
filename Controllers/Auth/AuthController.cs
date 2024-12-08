using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Models;
using OnlineShop.Models.DTOs;
using OnlineShop.Services.Interfaces;

namespace OnlineShop.Controllers.Auth;

[ApiController]
[Route("auth")]
/* [ApiExplorerSettings(GroupName = "auth")] */
public class AuthController(IConfiguration configuration, IUserService userService) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IUserService _userService = userService;

    [HttpPost("signup")]
    public async Task<ActionResult> SignUp(UserCreationDto userCreationDto)
    {
        var user = await _userService.GetUserByEmail(userCreationDto.Email);
        if (user != null)
        {
            return BadRequest("User already exist");
        }

        // there is i should hash user's password
        await _userService.AddUser(userCreationDto);
        return Ok("User was successfully signed up");
    }

    [HttpPost("signin")]
    public async Task<ActionResult> SignIn(UserCreationDto userDto)
    {
        var user = await _userService.GetUserByEmail(userDto.Email);
        if (user == null || user.Password != userDto.Password || user.Email != userDto.Email)
        {
            return Unauthorized("Invalid password/credentials");
        }

        string token = GenerateToken(user);

        return Ok(new { token });
    }

    /* [HttpPost("login")] */
    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim("Id", user.Id.ToString()),
        };

        var securitykey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!)
        );
        var creds = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims,
            expires: DateTime.Now.AddYears(7),
            signingCredentials: creds
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        string jwt = tokenHandler.WriteToken(token);

        return jwt;
    }
}
