using System;
using System.Linq;

namespace Connectied.Application.Guests;
public record GuestGroupDetailsDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public IReadOnlyCollection<GuestDto>? Guests { get; set; }
}
