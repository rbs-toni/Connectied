using Connectied.Application.Common.Specifications;
using Connectied.Domain.GuestList;
using System;
using System.Linq;

namespace Connectied.Application.GuestList.Queries;
class SearchGuestListSpecs : EntitiesByPaginationFilterSpec<Guest, GuestDto>
{
    public SearchGuestListSpecs(SearchGuestList filter) : base(filter)
    {
    }
}
