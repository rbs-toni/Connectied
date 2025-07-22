using Connectied.Domain.GuestLists;
using System;
using System.Linq;

namespace Connectied.Domain.Events;
public class GuestListCreatedEvent : BaseEvent
{
    public GuestListCreatedEvent(GuestList guestList)
    {
        GuestList = guestList;
    }

    public GuestList GuestList { get; }
}
