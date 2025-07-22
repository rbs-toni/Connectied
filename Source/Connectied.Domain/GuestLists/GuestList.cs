using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Connectied.Domain.GuestLists;
[Table("GuestLists", Schema = "Connectied")]
public class GuestList : BaseEntity, IAggregateRoot, IConcurrency
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    GuestList()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }
    public GuestList(string name)
    {
        Name = name;
        LinkCode = GenerateLinkCode();
    }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(16)]
    public string LinkCode { get; private set; }

    public GuestListConfiguration? Configuration { get; set; }

    [Timestamp]
    [NotNull]
    public byte[]? Version { get; set; }

    static string GenerateLinkCode()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string([.. Enumerable.Repeat(chars, 10).Select(s => s[Random.Shared.Next(s.Length)])]);
    }
}
