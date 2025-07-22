using Ardalis.Specification;
using Connectied.Domain.Guests;

namespace Connectied.Application.Dashboard;
class GetGuestStatsSpecs : Specification<Guest>
{
    public GetGuestStatsSpecs()
    {
        Query.AsNoTracking();
    }
}
