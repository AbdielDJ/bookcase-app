using System.Security.Claims;
using BackEnd.Modules.Auth.Dtos;
using BackEnd.Modules.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Modules.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService auth, IJwtService jwt) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = await auth.RegisterAsync(dto);
        if (user is null)
            return Conflict(new { message = "Ese correo ya está registrado." });

        return StatusCode(StatusCodes.Status201Created, user);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await auth.LoginAsync(dto);
        if (token is null)
            return Unauthorized(new { message = "Correo o contraseña incorrectos." });

        // El JWT viaja en una cookie HttpOnly: JavaScript no puede leerla (protege contra XSS)
        Response.Cookies.Append(AuthModule.CookieName, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(jwt.ExpiresMinutes),
        });

        return Ok(new { message = "Sesión iniciada." });
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(AuthModule.CookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax,
        });

        return NoContent();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(idClaim, out var id)) return Unauthorized();

        var user = await auth.GetByIdAsync(id);
        return user is null ? Unauthorized() : Ok(user);
    }
}
