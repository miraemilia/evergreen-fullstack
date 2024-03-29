namespace Evergreen.Service.src.DTO;

public class ProductUpdateDTO
{
    public string? Title { get; set; }
    public string? LatinName { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public Guid? CategoryId { get; set; }
}