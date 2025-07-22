using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Connectied.Domain.Guests;
[Table("Guests", Schema = "Connectied")]
public class Guest : BaseEntity, IAggregateRoot, IConcurrency
{
    readonly List<Guest> _members = [];
    readonly List<GuestRegistry> _registries = [];

    [MaxLength(100)]
    [ColumnViewable]
    public required string Name { get; set; }

    [EmailAddress]
    [ColumnViewable]
    public string? Email { get; set; }

    [Phone]
    [ColumnViewable]
    public string? PhoneNumber { get; set; }

    public string? GroupId { get; set; }
    [ForeignKey(nameof(GroupId))]
    public GuestGroup? Group { get; set; }

    public int Event1Quota { get; set; }
    public int Event2Quota { get; set; }
    public int Event1RSVP { get; set; }
    public int Event2RSVP { get; set; }
    public int Event1Attendance { get; set; }
    public int Event2Attendance { get; set; }
    public int Event1Angpao { get; set; }
    public int Event2Angpao { get; set; }
    public int Event1Gift { get; set; }
    public int Event2Gift { get; set; }
    public int Event1Souvenir { get; set; }
    public int Event2Souvenir { get; set; }
    public GuestRSVPStatus Event1RSVPStatus { get; set; }
    public GuestRSVPStatus Event2RSVPStatus { get; set; }

    public string? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    public Guest? Parent { get; set; }
    public IReadOnlyCollection<Guest> Members => _members.AsReadOnly();
    public ICollection<GuestRegistry> Registries => _registries.AsReadOnly();
    public string? Notes { get; set; }

    public bool CheckedIn { get; set; }
    [Timestamp]
    [NotNull]
    public byte[]? Version { get; set; }

    public void AddMember(Guest member)
    {
        if (_members.Contains(member))
        {
            return;
        }
        _members.Add(member);
        member.Parent = this;
        member.ParentId = Id;
    }
    public void AddRegistry(GuestRegistry registry)
    {
        _registries.Add(registry);
    }
    public void CheckIn()
    {
        if (CheckedIn)
        {
            return;
        }
        CheckedIn = true;
    }
}
