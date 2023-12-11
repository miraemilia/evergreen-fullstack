namespace Evergreen.Core.src.Entity;

public class Image
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public string? Description { get; set; }
    public IEnumerable<Product> Products { get; set; }
    public IEnumerable<Category> Categories { get; set; }
}