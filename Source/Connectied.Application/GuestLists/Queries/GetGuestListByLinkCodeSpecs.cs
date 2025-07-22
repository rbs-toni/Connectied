using Ardalis.Specification;
using Connectied.Domain.GuestLists;

namespace Connectied.Application.GuestLists.Queries;
class GetGuestListByLinkCodeSpecs : Specification<GuestList>
{
    public GetGuestListByLinkCodeSpecs(string linkCode)
    {
        Query.AsNoTracking()
            .Include(x => x.Configuration)
            .Where(x => x.LinkCode == linkCode);
    }
}
