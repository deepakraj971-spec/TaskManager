// TaskManager.Api/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Services;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;
    public AuthController(AuthService auth) => _auth = auth;

    public record LoginRequest(string Email, string Password);
    public record RegisterRequest(string Email, string Password);

    [HttpPost("register")]
    public async Task<ActionResult<bool>> Register([FromBody] RegisterRequest req)
        => Ok(await _auth.RegisterAsync(req.Email, req.Password));

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest req)
    {
        var token = await _auth.LoginAsync(req.Email, req.Password);
        return Ok(new { token });
    }
}
