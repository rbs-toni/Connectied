using System;
using Mapster;

namespace Connectied.Application.Common.Paging;
public record PagedList<T>(IReadOnlyCollection<T> Items, int Page, int PageSize, int TotalCount) : IPagedList<T>
    where T : class
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
    public IPagedList<TR> MapTo<TR>(Func<T, TR> map)
        where TR : class
    {
        return new PagedList<TR>(Items.Select(map).ToList(), Page, PageSize, TotalCount);
    }
    public IPagedList<TR> MapTo<TR>()
        where TR : class
    {
        return new PagedList<TR>(Items.Adapt<IReadOnlyCollection<TR>>(), Page, PageSize, TotalCount);
    }
}
