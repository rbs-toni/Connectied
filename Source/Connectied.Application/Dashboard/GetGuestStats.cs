using Ardalis.Result;
using Connectied.Application.Contracts;
using System;
using System.Linq;

namespace Connectied.Application.Dashboard;
public record GetGuestStats : IQuery<Result<GuestStats>>
{
}
