using System.Text.Json.Serialization;

namespace Evergreen.Core.src.Entity;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    [JsonIgnore]
    public List<Product> Products { get; set; }
}