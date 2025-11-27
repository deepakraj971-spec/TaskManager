// TaskManager.Application/Services/AuthService.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services;

public class AuthService
{
    private readonly UserManager<AppUser> _users;
    private readonly IConfiguration _config;

    public AuthService(UserManager<AppUser> users, IConfiguration config)
    {
        _users = users;
        _config = config;
    }

    public async Task<bool> RegisterAsync(string email, string password)
    {
        var user = new AppUser { UserName = email, Email = email };
        var result = await _users.CreateAsync(user, password);
        if (!result.Succeeded) throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        return true;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _users.FindByEmailAsync(email) ?? throw new UnauthorizedAccessException("Invalid credentials.");
        var valid = await _users.CheckPasswordAsync(user, password);
        if (!valid) throw new UnauthorizedAccessException("Invalid credentials.");
        return await GenerateTokenAsync(user);
    }

    private Task<string> GenerateTokenAsync(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? "")
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
            expires: DateTime.UtcNow.AddHours(8), signingCredentials: creds);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
