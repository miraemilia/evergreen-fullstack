using Evergreen.Core.src.Enum;

namespace Evergreen.Core.src.Entity;

public class Order : BaseEntity
{
    public User User { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public IEnumerable<OrderProduct> OrderDetails { get; set; }
}