using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Queries;
public record GetGuestLists : IQuery<Result<IReadOnlyCollection<GuestListDto>>>
{
}
