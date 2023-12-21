using Evergreen.Core.src.Entity;

namespace Evergreen.Service.src.DTO;

public class BasePageableReadDTO<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int Pages { get; set; }
}