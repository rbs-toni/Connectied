using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Queries;
public record GetGuestListWithGuests : IQuery<Result<GuestListWithGuestsDto>>
{
    public GetGuestListWithGuests(string id)
    {
        Id = id;
    }

    public string Id { get; }
}
