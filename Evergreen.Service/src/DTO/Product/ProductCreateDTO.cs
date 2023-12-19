using Evergreen.Core.src.Entity;

namespace Evergreen.Service.src.DTO;

public class ProductCreateDTO
{
    public string Title { get; set; }
    public string? LatinName { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public IEnumerable<ImageCreateDTO> ProductImages { get; set; }
    public ProductDetailsCreateDTO? ProductDetails { get; set; }
    public int Inventory { get; set; }
}