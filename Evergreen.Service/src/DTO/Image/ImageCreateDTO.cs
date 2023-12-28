namespace Evergreen.Service.src.DTO;

public class ImageCreateDTO
{
    public Guid ProductId { get; set; }
    public string ImageUrl { get; set; }
    public string? Description { get; set; }
}