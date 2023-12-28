namespace Evergreen.Core.src.Entity;

public class Image : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public string ImageUrl { get; set; }
    public string? Description { get; set; }
}