using Connectied.Domain.Guests;

namespace Connectied.Application.Guests;
public record GuestRegistryDto
{
    public GuestRegistryType Type { get; set; }
    public int Quantity { get; set; }
}
