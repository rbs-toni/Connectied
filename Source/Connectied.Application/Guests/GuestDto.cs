using System;
using System.Linq;
using Connectied.Domain.Guests;

namespace Connectied.Application.Guests;
public record GuestDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public GuestGroupDto? Group { get; set; }
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
    public bool Event1CheckedIn { get; set; }
    public bool Event2CheckedIn { get; set; }
    public GuestRSVPStatus Event1RSVPStatus { get; set; }
    public GuestRSVPStatus Event2RSVPStatus { get; set; }
    public string? Notes { get; set; }
}
