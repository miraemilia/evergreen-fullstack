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

    //[Authorize]
    [HttpGet()]
    public ActionResult<IEnumerable<UserReadDTO>> GetAll([FromQuery] GetAllParams options)
    {
        return Ok(_userService.GetAllUsers(options));
    }
}