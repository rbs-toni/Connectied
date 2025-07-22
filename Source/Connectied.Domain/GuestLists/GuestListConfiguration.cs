using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Connectied.Domain.GuestLists;
[Table("GuestListConfigurations", Schema = "Connectied")]
public class GuestListConfiguration : BaseEntity, IConcurrency
{
    public ICollection<string>? Columns { get; set; }
    public ICollection<string>? Groups { get; set; }
    public ICollection<string>? IncludedGuests { get; set; }
    public ICollection<string>? ExcludedGuests { get; set; }

    [Timestamp]
    [NotNull]
    public byte[]? Version { get; set; }
}
