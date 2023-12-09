namespace Evergreen.Core.src.Entity;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public Image Image { get; set; }
    public List<Product> Products { get; set; }
}