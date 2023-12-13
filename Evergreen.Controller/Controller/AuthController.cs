using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evergreen.Controller.src.Controller;

[ApiController]
[Route("api/v1/profile")]
public class AuthController : ControllerBase
{

    private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginParams loginParams)
    {
        return Ok(await _authService.Login(loginParams));
    }
}