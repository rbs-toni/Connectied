using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.GuestList.Queries;
public record GetGuestList : IQuery<Result<IReadOnlyCollection<GuestDto>>>
{
}
