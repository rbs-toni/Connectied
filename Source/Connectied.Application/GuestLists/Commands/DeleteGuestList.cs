using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Commands;
public record DeleteGuestList : IQuery<Result>
{
    public DeleteGuestList(string id)
    {
        Id = id;
    }
    public string Id { get; set; }
}
