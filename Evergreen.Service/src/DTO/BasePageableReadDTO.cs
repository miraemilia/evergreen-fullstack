namespace Evergreen.Service.src.DTO;

public class BasePageableReadDTO<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int? Page { get; set; }
}