using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;

namespace Evergreen.Service.src.DTO;

public class OrderCreateDTO
{
        public IEnumerable<OrderProductCreateDTO> OrderDetails { get; set; }
}