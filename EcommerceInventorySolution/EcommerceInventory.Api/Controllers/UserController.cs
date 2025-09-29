using EcommerceInventory.Application.DTO;
using EcommerceInventory.Application.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceInventory.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Application.DTO.RegisterRequest request)
    {
        var user = await _userService.RegisterUserAsync(request.Username, request.Password);
        return Ok(new { user.Id, user.Username });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Application.DTO.LoginRequest request)
    {
        var session = await _userService.LoginAsync(request.Username, request.Password, request.DeviceId);
        return Ok(new { session.Id, session.DeviceId, session.ExpiresAt });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        await _userService.LogoutAsync(request.UserId, request.SessionId);
        return Ok();
    }

    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAll([FromBody] LogoutAllRequest request)
    {
        await _userService.LogoutAllAsync(request.UserId);
        return Ok();
    }
}

