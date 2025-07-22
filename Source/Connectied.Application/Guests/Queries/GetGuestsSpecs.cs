using Ardalis.Specification;
using Connectied.Domain.Guests;

namespace Connectied.Application.Guests.Queries;

class GetGuestsSpecs : Specification<Guest>
{
    public GetGuestsSpecs()
    {
        Query.AsNoTracking()
            .Include(x => x.Group);
    }
}
