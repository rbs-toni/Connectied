using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.GuestList.Commands;
public class DeleteGuest : ICommand<Result>
{
    public DeleteGuest(string id)
    {
        Id = id;
    }

    public string Id { get; }
}
