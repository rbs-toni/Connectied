using Ardalis.Result;
using Connectied.Application.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace Connectied.Application.Guests.Commands;

public record CheckInEvent2 : ICommand<Result<string>>
{
    [NotNull]
    public string? Id { get; set; }

    public List<GuestRegistryDto>? Registries { get; set; }
}
