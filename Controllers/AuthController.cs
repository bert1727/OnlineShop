using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace onlineShop.Controllers;

[ApiController]
[Route("api")]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;

    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] LoginRequest loginRequest)
    {
        // Пример валидации пользователя
        if (loginRequest.Username != "user" || loginRequest.Password != "password")
        {
            return Unauthorized();
        }

        /* var claims = new[] */
        /* { */
        /*     new Claim(ClaimTypes.Name, loginRequest.Username), */
        /*     new Claim(ClaimTypes.Role, "Admin"), */
        /*     // Добавьте роли или другие утверждения */
        /* }; */


        /* var claims = new[] */
        /* { */
        /*     new Claim(JwtRegisteredClaimNames.Sub, loginRequest.Username), */
        /*     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), */
        /*     new Claim("Role", "Admin"), */
        /* }; */

        /* var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)); */
        /* var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); */


        /* var token = new JwtSecurityToken( */
        /*     issuer: jwtSettings["Issuer"], */
        /*     audience: jwtSettings["Audience"], */
        /*     claims: claims, */
        /*     expires: DateTime.Now.AddDays(1), */
        /*     signingCredentials: creds */
        /* ); */
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, loginRequest.Username),
            new Claim(ClaimTypes.Role, "Admin"),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securitykey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!)
        );
        var creds = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

        /* var tokenDescriptor = new SecurityTokenDescriptor */
        /* { */
        /* Subject = new ClaimsIdentity( */
        /*     [ */
        /*         new Claim(JwtRegisteredClaimNames.Sub, "testuser"), */
        /*         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), */
        /*     ] */
        /* ), */
        /*     Expires = DateTime.UtcNow.AddYears(7), */
        /*     Issuer = _configuration["JwtSettings:Issuer"], */
        /*     Audience = _configuration["JwtSettings:Audience"], */
        /*     SigningCredentials = creds, */
        /* }; */
        /* var token = tokenHandler.CreateToken(tokenDescriptor); */


        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims,
            expires: DateTime.Now.AddYears(7),
            signingCredentials: creds
        );

        string jwt = tokenHandler.WriteToken(token);

        Console.WriteLine(
            $"data: {_configuration["JwtSettings:Issuer"]} {_configuration["JwtSettings:Audience"]} {securitykey}"
        );
        /* Console.WriteLine( */
        /*     $"descriptor {tokenDescriptor.Audience} {tokenDescriptor.Issuer} {tokenDescriptor.Subject} {tokenDescriptor.Expires} {tokenDescriptor.SigningCredentials}" */
        /* ); */

        Console.WriteLine("Траяляляляля");
        tokenHandler.ValidateToken(
            jwt,
            new TokenValidationParameters
            {
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!)
                ),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            },
            out var validatedToken
        );
        /* Console.WriteLine( */
        /*     $"Is token valid: {validatedToken}, signing key: {token.SigningKey}, Security key: {token.SecurityKey}" */
        /* ); */
        /* Console.WriteLine(DateTime.UtcNow.AddDays(-7)); */
        /* Console.WriteLine( */
        /*     $"{tokenHandler.ReadToken(jwt)}, lalalaalal: {tokenHandler.ReadToken(tokenHandler.WriteToken(validatedToken))}" */
        /* ); */

        return Ok(jwt);
    }
}

public class LoginRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
