using Ardalis.Result;
using Ardalis.Specification;
using Connectied.Application.Contracts;
using Connectied.Domain.Guests;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Queries;
public record GetGuests : IQuery<Result<IReadOnlyCollection<GuestDto>>>
{
}

public class GetGuestsSpecs : Specification<Guest>
{
    public GetGuestsSpecs()
    {
        Query
            .Include(g => g.Group)
            .OrderBy(g => g.Name);
    }
}
