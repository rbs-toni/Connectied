using Ardalis.Specification;
using Connectied.Domain.GuestLists;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Queries;
class GetGuestListSpecs : Specification<GuestList>
{
    public GetGuestListSpecs()
    {
        Query.AsNoTracking()
            .Include(x => x.Configuration);
    }
}
