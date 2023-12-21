using System.Data.Common;
using System.Security.Claims;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
    [HttpPatch("profile/{id:Guid}")]
    public async Task<ActionResult<UserReadDTO>> UpdateProfile([FromRoute] Guid id, [FromBody] UserUpdateDTO updates)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (userId == id.ToString())
        {
            return Ok(await _authService.UpdateProfileAsync(id, updates));
        }
        else 
        {
            return Unauthorized("A user can only update their own profile.");
        }
    }

    [Authorize]
    [HttpPatch("profile/password/{id:Guid}")]
    public async Task<ActionResult<UserReadDTO>> ChangePassword([FromRoute] Guid id, [FromBody] ChangePasswordParams password)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (userId == id.ToString())
        {
            return Ok(await _authService.ChangePasswordAsync(id, password.Password));
        }
        return Unauthorized("A user can only change their own password.");
    }

    [Authorize]
    [HttpDelete("profile/{id:Guid}")]
    public async Task<ActionResult<bool>> Unregister([FromRoute] Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (userId == id.ToString())
        {
            return Ok(await _authService.DeleteProfileAsync(id));
        }
        return Unauthorized("A user can only change their own password.");
    }

}