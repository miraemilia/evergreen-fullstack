using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
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

    [HttpPost("login")]
    public ActionResult<string> Login([FromBody] LoginDTO loginDTO)
    {
        return Ok(_userService.Login(loginDTO));
    }

    //[Authorize]
    [HttpGet()]
    public ActionResult<IEnumerable<UserReadDTO>> GetAll([FromQuery] GetAllParams options)
    {
        return Ok(_userService.GetAllUsers(options));
    }

    //[Authorize]
    [HttpGet("{id:Guid}")]
    public ActionResult<UserReadDTO> GetOne([FromRoute] Guid id)
    {
        return Ok(_userService.GetUserById(id));
    }

    [HttpPost()]
    public ActionResult<UserReadDTO> CreateOne([FromBody] UserCreateDTO userCreateDTO)
    {
        return CreatedAtAction(nameof(CreateOne), _userService.CreateUser(userCreateDTO));
    }
}