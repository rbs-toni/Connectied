using Ardalis.Result;
using Connectied.Application.Common.Paging;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Queries;
public class SearchGuests : PaginationFilter, IQuery<Result<PagedList<GuestDto>>>
{
}
