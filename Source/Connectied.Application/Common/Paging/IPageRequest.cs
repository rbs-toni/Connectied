
namespace Connectied.Application.Common.Paging;
public interface IPageRequest
{
    int? Page { get; }
    int? PageSize { get; }
    string? Filters { get; }
    string[]? OrderBy { get; }

    PaginationFilter ToPaginationFilter();
}

public abstract record PageRequest : IPageRequest
{
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 100;
    public string? Filters { get; set; }
    public string[]? OrderBy { get; set; }

    public PaginationFilter ToPaginationFilter()
    {
        return new PaginationFilter()
        {
            Page = Page,
            PageSize = PageSize,
            Keyword = Filters,
            OrderBy = OrderBy
        };
    }
}
