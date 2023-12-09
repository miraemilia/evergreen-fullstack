using System.Net.Http.Headers;

namespace Evergreen.Core.src.Entity;

public class Product : BaseEntity
{
    public string Title { get; set; }
    public string LatinName { get; set; }
    public float Price { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public ProductDetails ProductDetails { get; set; }
    public IEnumerable<OrderProduct> OrderDetails { get; set; }
}