using Ardalis.Specification;
using Connectied.Application.Common.Specifications;
using Connectied.Domain.Guests;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Queries;
class SearchGuestsSpecs : EntitiesByPaginationFilterSpec<Guest, GuestDto>
{
    public SearchGuestsSpecs(SearchGuests filter) : base(filter)
    {
        Query.AsNoTracking();
    }
}
