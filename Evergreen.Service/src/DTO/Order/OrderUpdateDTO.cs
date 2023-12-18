using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;

namespace Evergreen.Service.src.DTO;

public class OrderUpdateDTO
{
    public OrderStatus OrderStatus { get; set; }
}