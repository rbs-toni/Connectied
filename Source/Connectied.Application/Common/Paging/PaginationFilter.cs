
namespace Connectied.Application.Common.Paging;
public class PaginationFilter : BaseFilter
{
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = -1;
    public string[]? OrderBy { get; set; }
}

public static class PaginationFilterExtensions
{
    public static bool HasOrderBy(this PaginationFilter filter)
    {
        return filter.OrderBy?.Any() is true;
    }
}
