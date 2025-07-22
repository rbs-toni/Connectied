using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Queries;
public record GetGuestGroups : IQuery<Result<IReadOnlyCollection<GuestGroupDto>>>
{
}
