using Evergreen.Core.src.Entity;

namespace Evergreen.Service.src.DTO;

public class ProductUpdateDTO
{
    public string? Title { get; set; }
    public string? LatinName { get; set; }
    public decimal? Price { get; set; }
    public Guid? CategoryId { get; set; }
    public IEnumerable<string>? ImageUrls { get; set; }
    public ProductDetailsUpdateDTO? ProductDetails { get; set; }
}