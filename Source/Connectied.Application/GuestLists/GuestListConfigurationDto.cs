using System;
using System.Linq;

namespace Connectied.Application.GuestLists;
public record GuestListConfigurationDto
{
    public ICollection<string>? Columns { get; set; }
    public ICollection<string>? Groups { get; set; }
    public ICollection<string>? IncludedGuests { get; set; }
    public ICollection<string>? ExcludedGuests { get; set; }
}
