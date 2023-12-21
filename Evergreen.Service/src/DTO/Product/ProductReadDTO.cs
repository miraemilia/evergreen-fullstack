namespace Evergreen.Service.src.DTO;

public class ProductReadDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? LatinName { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public CategoryReadDTO Category { get; set; }
    public IEnumerable<ProductImageReadDTO> ProductImages { get; set; }
    public ProductDetailsReadDTO? ProductDetails { get; set; }
    public int Inventory { get; set; }
}