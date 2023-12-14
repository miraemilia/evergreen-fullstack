using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evergreen.Controller.src.Controller;

[ApiController]
[Route("api/v1/[controller]s")]
public class UserController : ControllerBase
{
    private IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet()]
    public async Task<ActionResult<IEnumerable<UserReadDTO>>> GetAll([FromQuery] GetAllParams options)
    {
        return Ok(await _userService.GetAllUsersAsync(options));
    }

    [Authorize]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<UserReadDTO>> GetOne([FromRoute] Guid id)
    {
        return Ok(await _userService.GetUserByIdAsync(id));
    }

    [AllowAnonymous]
    [HttpPost()]
    public async Task<ActionResult<UserReadDTO>> CreateOne([FromBody] UserCreateDTO userCreateDTO)
    {
        return CreatedAtAction(nameof(CreateOne), await _userService.CreateUserAsync(userCreateDTO));
    }

    [Authorize]
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<bool>> DeleteOne([FromRoute] Guid id)
    {
        return Ok(await _userService.DeleteUserAsync(id));
    }

    [Authorize]
    [HttpPut("{id:Guid}")]
    public async Task<ActionResult<UserReadDTO>> UpdateOne([FromRoute] Guid id, [FromBody] UserUpdateDTO updates)
    {
        return Ok(await _userService.UpdateUserAsync(id, updates));
    }

    [AllowAnonymous]
    [HttpPost("email")]
    public async Task<ActionResult<bool>> EmailAvailable([FromBody] string email)
    {
        return Ok(await _userService.EmailAvailableAsync(email));
    }
}