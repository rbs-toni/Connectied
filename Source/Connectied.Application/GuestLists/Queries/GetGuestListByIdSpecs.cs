using Ardalis.Specification;
using Connectied.Domain.GuestLists;

namespace Connectied.Application.GuestLists.Queries;
class GetGuestListByIdSpecs : Specification<GuestList>
{
    public GetGuestListByIdSpecs(string id)
    {
        Query
            .AsNoTracking()
            .Include(x => x.Configuration)
            .Where(gl => gl.Id == id);
    }
}

class GetGuestListByCodeSpecs : Specification<GuestList>
{
    public GetGuestListByCodeSpecs(string code)
    {
        Query
            .AsNoTracking()
            .Include(x => x.Configuration)
            .Where(gl => gl.LinkCode == code);
    }
}
