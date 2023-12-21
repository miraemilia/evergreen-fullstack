using System.Security.Claims;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evergreen.Controller.src.Controller;

[ApiController]
[Route("api/v1/[controller]s")]
public class OrderController : ControllerBase
{
    private IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize (Roles = "Admin")]
    [HttpGet()]
    public async Task<ActionResult<IEnumerable<OrderReadDTO>>> GetAll([FromQuery] GetAllParams options)
    {
        return Ok(await _orderService.GetAllOrdersAsync(options));
    }

    [Authorize]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<OrderReadDTO>> GetOne([FromRoute] Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var admin = User.IsInRole("Admin");
        if (userId == id.ToString() || admin)
        {
            return Ok(await _orderService.GetOrderByIdAsync(id));
        }
        return Unauthorized("Not authorized to delete this order.");
    }

    [Authorize (Roles = "Customer")]
    [HttpPost()]
    public async Task<ActionResult<OrderReadDTO>> CreateOne([FromBody] OrderCreateDTO orderCreateDTO)
    {
        return CreatedAtAction(nameof(CreateOne), await _orderService.CreateOrderAsync(orderCreateDTO));
    }

    [Authorize (Roles = "Admin")]
    [HttpPatch("{id:Guid}")]
    public async Task<ActionResult<OrderReadDTO>> UpdateOne([FromRoute] Guid id, [FromBody] OrderUpdateDTO orderUpdateDTO)
    {
        return Ok(await _orderService.UpdateOrderStatusAsync(id, orderUpdateDTO));
    }

    [Authorize]
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<bool>> DeleteOne([FromRoute] Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var admin = User.IsInRole("Admin");
        if (userId == id.ToString() || admin)
        {
            return Ok(await _orderService.DeleteOrderAsync(id));
        }
        return Unauthorized("Not authorized to delete this order.");
    }

}