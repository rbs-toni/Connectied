using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Guests;

namespace Connectied.Application.GuestLists.Queries;
public record GetGuestsFromGuestList : IQuery<Result<GuestDto>>
{
    public GetGuestsFromGuestList(string linkCode)
    {
        LinkCode = linkCode;
    }

    public string LinkCode { get; }
}
