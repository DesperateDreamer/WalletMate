using Microsoft.AspNetCore.Mvc;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Shared.DTOs;

namespace WalletMate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await authService.LoginUserAsync(loginDto);
        return Ok(token);
    }
}