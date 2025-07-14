using System;

namespace Connectied.Application.Common.Paging;
public class PaginationResponse<T>
{
    public PaginationResponse(List<T> data, int count, int page, int pageSize)
    {
        Data = data;
        CurrentPage = page;
        PageSize = pageSize;
        TotalCount = count;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }
    public List<T> Data { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
}
