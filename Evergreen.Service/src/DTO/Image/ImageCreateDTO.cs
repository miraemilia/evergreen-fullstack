namespace Evergreen.Service.src.DTO;

public class ImageCreateDTO
{
    public string ImageUrl { get; set; }
    public string? Description { get; set; }
    public Guid ProductId { get; set; }
}