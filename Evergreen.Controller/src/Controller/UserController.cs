using Evergreen.Core.src.Enum;
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

    [Authorize (Roles= "Admin")]
    [HttpGet()]
    public async Task<ActionResult<UserPageableReadDTO>> GetAll([FromQuery] GetAllParams options)
    {
        return Ok(await _userService.GetAllUsersAsync(options));
    }

    [Authorize (Roles= "Admin")]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<UserReadDTO>> GetOne([FromRoute] Guid id)
    {
        return Ok(await _userService.GetUserByIdAsync(id));
    }

    [Authorize (Roles = "Admin")]
    [HttpPost()]
    public async Task<ActionResult<UserReadDTO>> CreateOne([FromBody] UserWithRoleCreateDTO userCreateDTO)
    {
        return CreatedAtAction(nameof(CreateOne), await _userService.CreateUserAsync(userCreateDTO));
    }

    [Authorize (Roles = "Admin")]
    [HttpPatch("{id:Guid}")]
    public async Task<ActionResult<UserReadDTO>> UpdateRole([FromRoute] Guid id, [FromBody] ChangeRoleParams role)
    {
        return Ok(await _userService.UpdateUserRoleAsync(id, role.Role));
    }

    [Authorize (Roles= "Admin")]
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<bool>> DeleteOne([FromRoute] Guid id)
    {
        return Ok(await _userService.DeleteUserAsync(id));
    }

    [AllowAnonymous]
    [HttpPost("emailAvailable")]
    public async Task<ActionResult<bool>> EmailAvailable([FromBody] string email)
    {
        return Ok(await _userService.EmailAvailableAsync(email));
    }

}