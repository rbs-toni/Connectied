using System;
using System.Linq;

namespace Connectied.Application.GuestLists;
public record GuestListDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? LinkCode { get; set; }

    public GuestListConfigurationDto? Configuration { get; set; }
}
