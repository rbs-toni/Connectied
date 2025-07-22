using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Guests;

namespace Connectied.Application.GuestLists.Queries;
public record GetGuestsFromGuestListById : IQuery<Result<IReadOnlyCollection<GuestDto>>>
{
    public GetGuestsFromGuestListById(string id)
    {
        Id = id;
    }
    public string Id { get; }
}
