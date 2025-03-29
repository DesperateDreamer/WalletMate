using Microsoft.AspNetCore.Mvc;
using WalletMate.BLL.Domain.Abstract;
using WalletMate.BLL.Domain.DTOs;

namespace WalletMate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var users = await userService.GetAllUsersAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(id, cancellationToken);

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken cancellationToken)
    {
        var newUserId = await userService.CreateUserAsync(dto, cancellationToken);

        return Ok(newUserId);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    {
        var updatedUserId = await userService.UpdateUserAsync(id, dto, cancellationToken);
        
        return Ok(updatedUserId);
    }
    
    [HttpPost("verify-credentials")]
    public async Task<IActionResult> VerifyCredentials([FromBody] VerifyCredentialsDto loginDto, CancellationToken cancellationToken)
    {
        var isValid = await userService.VerifyUserCredentialsAsync(loginDto.Email, loginDto.Password, cancellationToken);
        if (!isValid)
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok("Credentials verified successfully.");
    }
    
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken cancellationToken)
    {
        var result = await userService.ChangePasswordAsync(dto.UserId, dto.CurrentPassword, dto.NewPassword, cancellationToken);
        if (!result)
        {
            return BadRequest("Failed to change password. Check user ID and current password.");
        }

        return Ok("Password changed successfully.");
    }
}