using Ardalis.Specification;
using Connectied.Application.Common.Paging;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace Connectied.Application.Common.Specifications;
// See https://github.com/ardalis/Specification/issues/53
public static class SpecificationBuilderExtensions
{
    public static IOrderedSpecificationBuilder<T> AdvancedFilter<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Filter? filter)
    {
        if (filter is not null)
        {
            var parameter = Expression.Parameter(typeof(T));

            Expression binaryExpresioFilter;

            if (!string.IsNullOrEmpty(filter.Logic))
            {
                if (filter.Filters is null)
                {
                    throw new ArgumentNullException("The Filters attribute is required when declaring a logic");
                }

                binaryExpresioFilter = CreateFilterExpression(filter.Logic, filter.Filters, parameter);
            }
            else
            {
                var filterValid = GetValidFilter(filter);
                binaryExpresioFilter = CreateFilterExpression(
                    filterValid.Field!,
                    filterValid.Operator!,
                    filterValid.Value,
                    parameter);
            }

            ((List<WhereExpressionInfo<T>>)specificationBuilder.Specification.WhereExpressions)
                .Add(new WhereExpressionInfo<T>(Expression.Lambda<Func<T, bool>>(binaryExpresioFilter, parameter)));
        }

        return (IOrderedSpecificationBuilder<T>)specificationBuilder.Specification;
    }
    public static IOrderedSpecificationBuilder<T> AdvancedSearch<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Search? search)
    {
        if (!string.IsNullOrEmpty(search?.Keyword))
        {
            switch (search.Fields?.Any())
            {
                case true:
                    // search seleted fields (can contain deeper nested fields)
                    foreach (string field in search.Fields)
                    {
                        var paramExpr = Expression.Parameter(typeof(T));
                        MemberExpression propertyExpr = GetPropertyExpression(field, paramExpr);

                        specificationBuilder.AddSearchPropertyByKeyword(propertyExpr, paramExpr, search.Keyword);
                    }

                    break;

                default:
                    // search all fields (only first level)
                    foreach (var property in typeof(T).GetProperties()
                        .Where(
                            prop => (Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType) is { } propertyType &&
                                !propertyType.IsEnum &&
                                Type.GetTypeCode(propertyType) != TypeCode.Object))
                    {
                        var paramExpr = Expression.Parameter(typeof(T));
                        var propertyExpr = Expression.Property(paramExpr, property);

                        specificationBuilder.AddSearchPropertyByKeyword(propertyExpr, paramExpr, search.Keyword);
                    }

                    break;
            }
        }

        return (IOrderedSpecificationBuilder<T>)specificationBuilder;
    }
    public static dynamic? ChangeType(object value, Type conversion)
    {
        var t = conversion;

        if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        {
            if (value == null)
            {
                return null;
            }

            t = Nullable.GetUnderlyingType(t);
        }

        return Convert.ChangeType(value, t!);
    }
    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        string[]? orderByFields)
    {
        if (orderByFields is not null)
        {
            foreach (var field in ParseOrderBy(orderByFields))
            {
                var paramExpr = Expression.Parameter(typeof(T));

                Expression propertyExpr = paramExpr;
                foreach (string member in field.Key.Split('.'))
                {
                    propertyExpr = Expression.PropertyOrField(propertyExpr, member);
                }

                var keySelector = Expression.Lambda<Func<T, object?>>(
                    Expression.Convert(propertyExpr, typeof(object)),
                    paramExpr);

                ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions)
                    .Add(new OrderExpressionInfo<T>(keySelector, field.Value));
            }
        }

        return (IOrderedSpecificationBuilder<T>)specificationBuilder;
    }

    public static ISpecificationBuilder<T> PaginateBy<T>(this ISpecificationBuilder<T> query, PaginationFilter filter)
    {
        if (filter.Page.HasValue && filter.PageSize.HasValue)
        {
            // Ensure PageSize is positive, if not, return all data (no pagination)
            if (filter.PageSize <= 0)
            {
                return query; // No pagination applied
            }

            // Ensure Page is valid, default to 1 if it's less than or equal to 0
            if (filter.Page <= 0)
            {
                filter.Page = 1;  // Default to the first page
            }

            // Apply pagination logic if Page is valid and greater than 1
            query = query.Skip((filter.Page.Value - 1) * filter.PageSize.Value);

            return query.Take(filter.PageSize.Value);
        }
        return query;
    }
    public static ISpecificationBuilder<T> SearchBy<T>(this ISpecificationBuilder<T> query, BaseFilter filter) => query
            .SearchByKeyword(filter.Keyword)
        .AdvancedSearch(filter.AdvancedSearch)
        .AdvancedFilter(filter.AdvancedFilter);
    public static IOrderedSpecificationBuilder<T> SearchByKeyword<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        string? keyword) => specificationBuilder.AdvancedSearch(new Search { Keyword = keyword });

    static void AddSearchPropertyByKeyword<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression propertyExpr,
        ParameterExpression paramExpr,
        string keyword,
        string operatorSearch = FilterOperator.CONTAINS)
    {
        if (propertyExpr is not MemberExpression memberExpr || memberExpr.Member is not PropertyInfo property)
        {
            throw new ArgumentException("propertyExpr must be a property expression.", nameof(propertyExpr));
        }

        string searchTerm = operatorSearch switch
        {
            FilterOperator.STARTSWITH => $"{keyword.ToLower()}%",
            FilterOperator.ENDSWITH => $"%{keyword.ToLower()}",
            FilterOperator.CONTAINS => $"%{keyword.ToLower()}%",
            _ => throw new ArgumentException("operatorSearch is not valid.", nameof(operatorSearch))
        };

        // Generate lambda [ x => x.Property ] for string properties
        // or [ x => ((object)x.Property) == null ? null : x.Property.ToString() ] for other properties
        Expression selectorExpr =
            property.PropertyType == typeof(string)
            ? propertyExpr
            : Expression.Condition(
                Expression.Equal(
                    Expression.Convert(propertyExpr, typeof(object)),
                    Expression.Constant(null, typeof(object))),
                Expression.Constant(null, typeof(string)),
                Expression.Call(propertyExpr, "ToString", null, null));

        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
        Expression callToLowerMethod = Expression.Call(selectorExpr, toLowerMethod!);

        var selector = Expression.Lambda<Func<T, string>>(callToLowerMethod, paramExpr);

        ((List<SearchExpressionInfo<T>>)specificationBuilder.Specification.SearchCriterias)
            .Add(new SearchExpressionInfo<T>(selector, searchTerm, 1));
    }
    static Expression CombineFilter(string filterOperator, Expression bExpresionBase, Expression bExpresion) => filterOperator switch
    {
        FilterLogic.AND => Expression.And(bExpresionBase, bExpresion),
        FilterLogic.OR => Expression.Or(bExpresionBase, bExpresion),
        FilterLogic.XOR => Expression.ExclusiveOr(bExpresionBase, bExpresion),
        _ => throw new ArgumentException("FilterLogic is not valid."),
    };
    static Expression CreateFilterExpression(
        string logic,
        IEnumerable<Filter> filters,
        ParameterExpression parameter)
    {
        Expression filterExpression = default!;

        foreach (var filter in filters)
        {
            Expression bExpresionFilter;

            if (!string.IsNullOrEmpty(filter.Logic))
            {
                if (filter.Filters is null)
                {
                    throw new ArgumentNullException("The Filters attribute is required when declaring a logic");
                }

                bExpresionFilter = CreateFilterExpression(filter.Logic, filter.Filters, parameter);
            }
            else
            {
                var filterValid = GetValidFilter(filter);
                bExpresionFilter = CreateFilterExpression(
                    filterValid.Field!,
                    filterValid.Operator!,
                    filterValid.Value,
                    parameter);
            }

            filterExpression = filterExpression is null
                ? bExpresionFilter
                : CombineFilter(logic, filterExpression, bExpresionFilter);
        }

        return filterExpression;
    }
    static Expression CreateFilterExpression(
        Expression memberExpression,
        Expression constantExpression,
        string filterOperator)
    {
        if (memberExpression.Type == typeof(string))
        {
            constantExpression = Expression.Call(constantExpression, "ToLower", null);
            memberExpression = Expression.Call(memberExpression, "ToLower", null);
        }

        return filterOperator switch
        {
            FilterOperator.EQ => Expression.Equal(memberExpression, constantExpression),
            FilterOperator.NEQ => Expression.NotEqual(memberExpression, constantExpression),
            FilterOperator.LT => Expression.LessThan(memberExpression, constantExpression),
            FilterOperator.LTE => Expression.LessThanOrEqual(memberExpression, constantExpression),
            FilterOperator.GT => Expression.GreaterThan(memberExpression, constantExpression),
            FilterOperator.GTE => Expression.GreaterThanOrEqual(memberExpression, constantExpression),
            FilterOperator.CONTAINS => Expression.Call(memberExpression, "Contains", null, constantExpression),
            FilterOperator.STARTSWITH => Expression.Call(memberExpression, "StartsWith", null, constantExpression),
            FilterOperator.ENDSWITH => Expression.Call(memberExpression, "EndsWith", null, constantExpression),
            _ => throw new ArgumentNullException("Filter Operator is not valid."),
        };
    }
    static Expression CreateFilterExpression(
        string field,
        string filterOperator,
        object? value,
        ParameterExpression parameter)
    {
        var propertyExpresion = GetPropertyExpression(field, parameter);
        var valueExpresion = GeValuetExpression(field, value, propertyExpresion.Type);
        return CreateFilterExpression(propertyExpresion, valueExpresion, filterOperator);
    }
    static MemberExpression GetPropertyExpression(string propertyName, ParameterExpression parameter)
    {
        Expression propertyExpression = parameter;
        foreach (string member in propertyName.Split('.'))
        {
            propertyExpression = Expression.PropertyOrField(propertyExpression, member);
        }

        return (MemberExpression)propertyExpression;
    }
    static string GetStringFromJsonElement(object value) => ((JsonElement)value).GetString()!;
    static Filter GetValidFilter(Filter filter)
    {
        if (string.IsNullOrEmpty(filter.Field))
        {
            throw new ArgumentNullException("The field attribute is required when declaring a filter");
        }

        if (string.IsNullOrEmpty(filter.Operator))
        {
            throw new ArgumentNullException("The Operator attribute is required when declaring a filter");
        }

        return filter;
    }
    static ConstantExpression GeValuetExpression(string field, object? value, Type propertyType)
    {
        if (value == null)
        {
            return Expression.Constant(null, propertyType);
        }

        if (propertyType.IsEnum)
        {
            string? stringEnum = GetStringFromJsonElement(value);

            if (!Enum.TryParse(propertyType, stringEnum, true, out object? valueparsed))
            {
                throw new ArgumentNullException(string.Format("Value {0} is not valid for {1}", value, field));
            }

            return Expression.Constant(valueparsed, propertyType);
        }

        if (propertyType == typeof(Guid))
        {
            string? stringGuid = GetStringFromJsonElement(value);

            if (!Guid.TryParse(stringGuid, out Guid valueparsed))
            {
                throw new ArgumentNullException(string.Format("Value {0} is not valid for {1}", value, field));
            }

            return Expression.Constant(valueparsed, propertyType);
        }

        if (propertyType == typeof(string))
        {
            string? text = GetStringFromJsonElement(value);

            return Expression.Constant(text, propertyType);
        }

        if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
        {
            string? text = GetStringFromJsonElement(value);
            return Expression.Constant(ChangeType(text, propertyType), propertyType);
        }

        return Expression.Constant(ChangeType(((JsonElement)value).GetRawText(), propertyType), propertyType);
    }
    static Dictionary<string, OrderTypeEnum> ParseOrderBy(string[] orderByFields) => new(
        orderByFields.Select(
            (orderByfield, index) =>
            {
                string[] fieldParts = orderByfield.Split(' ');
                string field = fieldParts[0];
                bool descending = fieldParts.Length > 1 &&
                    fieldParts[1].StartsWith("Desc", StringComparison.OrdinalIgnoreCase);
                var orderBy = index == 0
                    ? descending ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
                    : descending ? OrderTypeEnum.ThenByDescending : OrderTypeEnum.ThenBy;

                return new KeyValuePair<string, OrderTypeEnum>(field, orderBy);
            }));
}
