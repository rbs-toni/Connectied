using Connectied.Domain.GuestLists;
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
    readonly List<GuestList> _guestLists = new List<GuestList>();
    readonly List<WeddingEvent> _events = new List<WeddingEvent>();
    public DateTime? Date { get; set; }
    [Timestamp]
    [NotNull]
    public byte[]? Version { get; set; }
    public IReadOnlyCollection<WeddingEvent> Events => _events.AsReadOnly();
    public IReadOnlyCollection<Guest> Guests => _guests.AsReadOnly();
    public IReadOnlyCollection<GuestList> GuestLists => _guestLists.AsReadOnly();
}
