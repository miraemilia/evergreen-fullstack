using Evergreen.Core.src.Enum;

namespace Evergreen.Core.src.Entity;

public class ProductDetails : BaseEntity
{
    public Product Product { get; set; }
    public Guid ProductId { get; set; }
    public ProductSize Size { get; set; }
    public DetailsOption Watering { get; set; }
    public DetailsOption Light { get; set; }
    public DetailsOption Difficulty { get; set; }
    public bool? Hanging { get; set; }
    public bool? NonToxic { get; set; }
    public bool? AirPurifying {get; set;}
}