using Evergreen.Core.src.Enum;

namespace Evergreen.Core.src.Parameter;

public class GetAllParams
{
    public int Limit { get; set; } = 24;
    public int Offset { get; set; } = 0;
    public Guid? Id { get; set; }
    public string Search { get; set; } = string.Empty;
    public SortOrder SortOrder { get; set; } = SortOrder.Asc;
    public SortCriterion SortCriterion { get; set; } = SortCriterion.CreatedAt;
}