using System;
using System.Linq;

namespace Connectied.Application.Guests;
public record GuestGroupDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
}
