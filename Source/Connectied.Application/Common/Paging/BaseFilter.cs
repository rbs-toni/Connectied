using System;

namespace Connectied.Application.Common.Paging;
public class BaseFilter
{
    /// <summary>
    /// Keyword to Search in All the available columns of the Resource.
    /// </summary>
    public string? Keyword { get; set; }

    /// <summary>
    /// Column Wise Search is Supported.
    /// </summary>
    public Search? AdvancedSearch { get; set; }

    /// <summary>
    /// Advanced column filtering with logical operators and query operators is supported.
    /// </summary>
    public Filter? AdvancedFilter { get; set; }
}
