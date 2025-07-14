using System;

namespace Connectied.Application.Common.Paging;
public class Search
{
    public List<string> Fields { get; set; } = [];
    public string? Keyword { get; set; }
}
