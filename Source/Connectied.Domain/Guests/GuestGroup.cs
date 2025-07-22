using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Connectied.Domain.Guests;
[Table("GuestGroups", Schema = "Connectied")]
public class GuestGroup : BaseEntity, IAggregateRoot, IConcurrency
{
    readonly List<Guest> _guests = [];

    [Required]
    [NotNull]
    [MaxLength(50)]
    public string? Name { get; set; }

    [Timestamp]
    [NotNull]
    public byte[]? Version { get; set; }

    public ICollection<Guest> Guests => _guests.AsReadOnly();
}
