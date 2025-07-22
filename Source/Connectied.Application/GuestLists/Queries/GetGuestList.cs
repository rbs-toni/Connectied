using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Guests;
using Connectied.Application.Repositories;
using Connectied.Domain.GuestLists;
using Connectied.Domain.Guests;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Queries;
public record GetGuestList : IQuery<Result<GuestListDto>>
{
    public GetGuestList(string id)
    {
        Id = id;
    }
    public string Id { get; }
}
