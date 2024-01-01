namespace Evergreen.Service.src.DTO;

public class OrderCreateDTO
{
        public IEnumerable<OrderProductCreateDTO> OrderDetails { get; set; }
}