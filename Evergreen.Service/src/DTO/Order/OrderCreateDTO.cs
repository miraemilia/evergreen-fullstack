using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;

namespace Evergreen.Service.src.DTO;

public class OrderCreateDTO
{
    public Guid UserId { get; set; }
    public IEnumerable<OrderProductCreateDTO> OrderDetails { get; set; }
}