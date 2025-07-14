using Ardalis.Result;
using Connectied.Application.Common.Paging;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.GuestList.Queries;
public class SearchGuestList : PaginationFilter, IQuery<Result<PagedList<GuestDto>>>
{
}
