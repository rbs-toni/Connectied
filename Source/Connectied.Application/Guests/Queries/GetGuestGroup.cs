using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Queries;
public record GetGuestGroup : IQuery<Result<GuestGroupDetailsDto>>
{
    public GetGuestGroup(string id)
    {
        Id = id;
    }
    public string Id { get; set; }
}
