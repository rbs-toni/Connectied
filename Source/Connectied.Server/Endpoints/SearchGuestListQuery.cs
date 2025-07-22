using System;
using System.Linq;
using System.Reflection;

namespace Connectied.Server.Endpoints;
class SearchGuestListQuery
{
    public required int Page { get; init; } = 1;
    public required int PageSize { get; init; } = 10;
    public string[]? OrderBy { get; init; }
    public string? Keyword { get; init; }
    public string? AdvancedSearch { get; init; }
    public string? AdvancedFilter { get; init; }

    public static ValueTask<SearchGuestListQuery?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        const string currentPageKey = "page";
        const string pageSizeKey = "pageSize";
        const string orderByKey = "orderBy";
        const string keywordKey = "keyword";

        var query = context.Request.Query;

        _ = int.TryParse(query[currentPageKey], out int page);
        _ = int.TryParse(query[pageSizeKey], out int pageSize);
        var orderBy = query[orderByKey].ToArray();
        var keyword = query[keywordKey].ToString();

        var result = new SearchGuestListQuery()
        {
            Page = page == 0 ? 1 : page,
            PageSize = pageSize == 0 ? 10 : pageSize,
            OrderBy = orderBy.Length > 0 ? orderBy : null,
            Keyword = keyword
        };

        return ValueTask.FromResult<SearchGuestListQuery?>(result);
    }
}
