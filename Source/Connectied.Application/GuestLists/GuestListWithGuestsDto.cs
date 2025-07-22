using System;
using System.Linq;
using Connectied.Application.Guests;

namespace Connectied.Application.GuestLists;
public record GuestListWithGuestsDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? LinkCode { get; set; }
    public IReadOnlyCollection<GuestDto>? Guests { get; set; }
    public GuestListConfigurationDto? Configuration { get; set; }
}
