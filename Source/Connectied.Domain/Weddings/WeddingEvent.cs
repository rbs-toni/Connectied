using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;

namespace Connectied.Domain.Weddings;
[Table("WeddingEvents", Schema = "Connectied")]
public class WeddingEvent : BaseEntity, IAggregateRoot, IConcurrency
{
    [MaxLength(100)]
    [ColumnViewable]
    public required string Name { get; set; }

    public DateTime? Date { get; set; }

    public string? WeddingId { get; set; }

    [ForeignKey(nameof(WeddingId))]
    public Wedding? Wedding { get; set; }

    [Timestamp]
    [NotNull]
    public byte[]? Version { get; set; }
}
