using Connectied.Domain.Guests;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Connectied.Domain.Weddings;
[Table("Weddings", Schema = "Connectied")]
public class Wedding : BaseEntity, IAggregateRoot, IConcurrency
{
    readonly List<Guest> _guests = new List<Guest>();
    public DateTime? Date { get; set; }
    [Timestamp]
    [NotNull]
    public byte[]? Version { get; set; }
    public IReadOnlyCollection<Guest> Guests => _guests.AsReadOnly();
}
