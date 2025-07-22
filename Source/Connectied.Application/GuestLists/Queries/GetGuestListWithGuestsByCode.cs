using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Queries;
public record GetGuestListWithGuestsByCode : IQuery<Result<GuestListWithGuestsDto>>
{
    public GetGuestListWithGuestsByCode(string code)
    {
        Code = code;
    }

    public string Code { get; }
}
