using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Domain.Guests;
using System.Diagnostics.CodeAnalysis;

namespace Connectied.Application.Guests.Commands;
public record UpdateGuestRSVP : ICommand<Result<string>>
{
    [NotNull]
    public string? Id { get; set; }
    public GuestRSVPStatus Event1Status { get; set; }
    public GuestRSVPStatus Event2Status { get; set; }
}
