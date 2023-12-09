namespace Evergreen.Core.src.Entity;

public class OrderProduct
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int quantity { get; set; }
}