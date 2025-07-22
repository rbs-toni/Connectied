using Ardalis.Specification;
using Connectied.Domain.GuestLists;
using Connectied.Domain.Guests;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Queries;
sealed class GetGuestsFromGuestListConfigurationSpecs : Specification<Guest>
{
    public GetGuestsFromGuestListConfigurationSpecs(GuestListConfiguration config)
    {
        Query
            .AsNoTracking()
            .Include(x => x.Group);

        var hasGroups = config.Groups != null && config.Groups.Any();
        var hasIncluded = config.IncludedGuests != null && config.IncludedGuests.Any();
        var hasExcluded = config.ExcludedGuests != null && config.ExcludedGuests.Any();

        if (hasGroups)
        {
            Query.Where(g => g.Group != null && config.Groups.Contains(g.Group.Id));
        }

        if (hasIncluded)
        {
            Query.Where(g => config.IncludedGuests.Contains(g.Id));
        }
        else if (hasExcluded)
        {
            Query.Where(g => !config.ExcludedGuests.Contains(g.Id));
        }
    }
}
