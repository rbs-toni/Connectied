using System;

namespace Connectied.Application.Common.Paging;
public static class FilterLogic
{
    public const string AND = "and";
    public const string OR = "or";
    public const string XOR = "xor";
}
public static class FilterOperator
{
    public const string CONTAINS = "contains";
    public const string ENDSWITH = "endswith";
    public const string EQ = "eq";
    public const string GT = "gt";
    public const string GTE = "gte";
    public const string LT = "lt";
    public const string LTE = "lte";
    public const string NEQ = "neq";
    public const string STARTSWITH = "startswith";
}
public class Filter
{
    public string? Field { get; set; }
    public IEnumerable<Filter>? Filters { get; set; }
    public string? Logic { get; set; }
    public string? Operator { get; set; }
    public object? Value { get; set; }
}
