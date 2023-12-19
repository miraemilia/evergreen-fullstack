namespace Evergreen.Core.src.Entity;

public class Product : BaseEntity
{
    public string Title { get; set; }
    public string? LatinName { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public IEnumerable<Image> ProductImages { get; set; }
    public ProductDetails? ProductDetails { get; set; }
    public int Inventory { get; set; }
    public IEnumerable<OrderProduct> OrderDetails { get; set; }
}