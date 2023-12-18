using Evergreen.Core.src.Enum;

namespace Evergreen.Service.src.DTO;

public class OrderReadDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public IEnumerable<OrderProductReadDTO> OrderDetails { get; set; }
}