using Evergreen.Core.src.Entity;

namespace Evergreen.Service.src.DTO;

public class CategoryUpdateDTO
{
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }

    public Category Merge(Category category)
    {
        if (Name is not null)
        {
            category.Name = Name;
        }
        if (ImageUrl is not null)
        {
            category.ImageUrl = ImageUrl;
        }
        return category;
    }
}