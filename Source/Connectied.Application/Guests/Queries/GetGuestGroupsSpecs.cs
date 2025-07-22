using Ardalis.Specification;
using Connectied.Domain.Guests;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Queries;
class GetGuestGroupsSpecs : Specification<GuestGroup>
{
    public GetGuestGroupsSpecs()
    {
        Query.AsNoTracking()
            .OrderBy(x => x.Name);
    }
}
