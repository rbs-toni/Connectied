using System;

namespace Connectied.Application.Common.Paging;
public interface IPagedList<T> where T : class
{
    int Page { get; init; }
    int PageSize { get; init; }
    IReadOnlyCollection<T> Items { get; init; }
    int TotalPages { get; }
    bool HasPrevious { get; }
    bool HasNext { get; }
    int TotalCount { get; init; }

    IPagedList<TR> MapTo<TR>(Func<T, TR> map)
        where TR : class;
    IPagedList<TR> MapTo<TR>()
       where TR : class;
}
