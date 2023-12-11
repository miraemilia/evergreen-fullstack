using System.ComponentModel.DataAnnotations;

namespace Evergreen.Core.src.Entity;

public class OrderProduct
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    [Range (1, 1000)]
    public int Quantity { get; set; }
}