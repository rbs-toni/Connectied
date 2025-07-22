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
    readonly List<GuestRegistry> _eventRegistries = [];

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
    public IReadOnlyCollection<GuestRegistry> EventRegistries => _eventRegistries.AsReadOnly();
    public string? Notes { get; set; }

    public bool Event1CheckedIn { get; set; }
    public bool Event2CheckedIn { get; set; }
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
    void AddEvent1Registry(GuestRegistry registry)
    {
        _eventRegistries.Add(registry);
        if (registry.Type == GuestRegistryType.Angpao)
        {
            Event1Angpao += registry.Quantity;
        }
        else if (registry.Type == GuestRegistryType.Gift)
        {
            Event1Gift += registry.Quantity;
        }
    }
    void AddEvent2Registry(GuestRegistry registry)
    {
        _eventRegistries.Add(registry);
        if (registry.Type == GuestRegistryType.Angpao)
        {
            Event2Angpao += registry.Quantity;
        }
        else if (registry.Type == GuestRegistryType.Gift)
        {
            Event2Gift += registry.Quantity;
        }
    }

    public void CheckInEvent1(List<GuestRegistry>? registries)
    {
        if (Event1CheckedIn)
        {
            return;
        }
        if (registries?.Count > 0)
        {
            foreach (var item in registries)
            {
                AddEvent1Registry(item);
            }
        }
        Event1CheckedIn = true;
        Event1Attendance++;
        Event1Souvenir++;
    }

    public void CheckInEvent2(List<GuestRegistry>? registries)
    {
        if (Event2CheckedIn)
        {
            return;
        }
        if (registries?.Count > 0)
        {
            foreach (var item in registries)
            {
                AddEvent2Registry(item);
            }
        }
        Event2CheckedIn = true;
        Event2Attendance++;
        Event2Souvenir++;
    }
}
