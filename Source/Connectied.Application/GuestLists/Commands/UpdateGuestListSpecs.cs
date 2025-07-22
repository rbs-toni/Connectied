using Ardalis.Specification;
using Connectied.Domain.GuestLists;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Commands;

sealed class UpdateGuestListSpecs : Specification<GuestList>
{
    public UpdateGuestListSpecs(string id)
    {
        Query
            .Include(x => x.Configuration)
            .Where(x => x.Id == id);
    }
}
