using Ardalis.Specification;
using Connectied.Domain.GuestLists;
using Connectied.Domain.Guests;
using System.Linq;

namespace Connectied.Application.GuestLists.Queries;
public class GuestsByGuestListConfigurationSpecs : Specification<Guest>
{
    public GuestsByGuestListConfigurationSpecs(GuestListConfiguration? config, bool excludeIncluded = false)
    {
        if (config == null)
        {
            return;
        }

        if (config.Groups?.Any() == true)
        {
            Query.Where(g => g.GroupId != null && config.Groups.Contains(g.GroupId));
        }

        if (config.ExcludedGuests?.Any() == true)
        {
            Query.Where(g => !config.ExcludedGuests.Contains(g.Id));
        }

        if (excludeIncluded && config.IncludedGuests?.Any() == true)
        {
            Query.Where(g => !config.IncludedGuests.Contains(g.Id));
        }
    }
}
