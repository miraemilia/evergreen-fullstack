namespace Evergreen.Core.src.Entity;

public class Image : BaseEntity
{
    public string ImageUrl { get; set; }
    public string? Description { get; set; }
    public IEnumerable<Product> Products { get; set; }
}