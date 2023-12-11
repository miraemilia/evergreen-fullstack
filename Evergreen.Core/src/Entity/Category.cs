namespace Evergreen.Core.src.Entity;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Image Image { get; set; }
    public List<Product> Products { get; set; }
}