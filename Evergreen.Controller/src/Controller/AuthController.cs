using System.Security.Claims;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evergreen.Controller.src.Controller;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{

    private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> Login([FromBody] LoginParams loginParams)
    {
        return Ok(await _authService.Login(loginParams));
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserReadDTO>> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        return Ok(await _authService.GetProfileAsync(Guid.Parse(userId)));
    }

    [AllowAnonymous]
    [HttpPost("profile")]
    public async Task<ActionResult<UserReadDTO>> Register([FromBody] UserCreateDTO userCreateDTO)
    {
        return CreatedAtAction(nameof(Register), await _authService.CreateProfileAsync(userCreateDTO));
    }

    [Authorize]
    [HttpPatch("profile")]
    public async Task<ActionResult<UserReadDTO>> UpdateProfile([FromBody] UserUpdateDTO updates)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        return Ok(await _authService.UpdateProfileAsync(Guid.Parse(userId), updates));
    }

    [Authorize]
    [HttpPatch("profile/change-password")]
    public async Task<ActionResult<UserReadDTO>> ChangePassword([FromBody] ChangePasswordParams password)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        return Ok(await _authService.ChangePasswordAsync(Guid.Parse(userId), password.Password));
    }

    [Authorize]
    [HttpDelete("profile")]
    public async Task<ActionResult<bool>> Unregister()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        return Ok(await _authService.DeleteProfileAsync(Guid.Parse(userId)));
    }

}