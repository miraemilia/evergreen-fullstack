using Evergreen.Core.src.Enum;

namespace Evergreen.Service.src.DTO;

public class ProductDetailsUpdateDTO
{
    public ProductSize? Size { get; set; }
    public DetailsOption? Watering { get; set; }
    public DetailsOption? Light { get; set; }
    public DetailsOption? Difficulty { get; set; }
    public bool? Hanging { get; set; }
    public bool? NonToxic { get; set; }
    public bool? AirPurifying {get; set;}
}