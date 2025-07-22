using System;
using System.Linq;

namespace Connectied.Application.Guests;
public record GuestDetailsDto : GuestDto
{
    public GuestDto? Parent { get; set; }
    public IReadOnlyCollection<GuestDetailsDto>? Members { get; set; }
    public IReadOnlyCollection<GuestRegistryDto>? EventRegistries { get; set; }
}
