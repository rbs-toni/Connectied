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

        var hasGroups = config.Groups != null && config.Groups.Count != 0;
        var hasIncluded = config.IncludedGuests != null && config.IncludedGuests.Count != 0;
        var hasExcluded = config.ExcludedGuests != null && config.ExcludedGuests.Count != 0;

        // If no groups and no included guests, return nothing
        if (!hasGroups && !hasIncluded)
        {
            Query.Where(_ => false);
            return;
        }

        // Include guests that are in groups or explicitly included
        Query.Where(g =>
            (hasGroups && g.Group != null && config.Groups.Contains(g.Group.Id)) ||
            (hasIncluded && config.IncludedGuests.Contains(g.Id))
        );

        // Exclude guests explicitly
        if (hasExcluded)
        {
            Query.Where(g => !config.ExcludedGuests.Contains(g.Id));
        }
    }
}
