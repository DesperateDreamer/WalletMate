using Microsoft.AspNetCore.Mvc;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Shared.DTOs;

namespace WalletMate.API.Controllers;

/// <summary>
/// Controller for managing authentication-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Authenticates a user and generates a JWT token.
    /// </summary>
    /// <param name="loginDto">The data transfer object containing login credentials.</param>
    /// <returns>A JWT token if authentication is successful.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await authService.LoginUserAsync(loginDto);
        return Ok(token);
    }
}