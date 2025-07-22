using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Queries;
public record GetGuests : IQuery<Result<IReadOnlyCollection<GuestDto>>>
{
}
