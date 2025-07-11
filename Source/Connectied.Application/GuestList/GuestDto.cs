using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Connectied.Application.GuestList;
public record GuestDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Group { get; set; }
    public int Event1Quota { get; set; }
    public int Event2Quota { get; set; }
    public int Event1Rsvp { get; set; }
    public int Event2Rsvp { get; set; }
    public int Event1Attend { get; set; }
    public int Event2Attend { get; set; }
    public int Event2AngpaoCount { get; set; }
    public int Event2GiftCount { get; set; }
    public int Event2Souvenir { get; set; }
    public string? Notes { get; set; }
}
