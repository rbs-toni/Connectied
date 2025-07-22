using Connectied.Domain.GuestLists;
using System;
using System.Linq;

namespace Connectied.Domain.Events;
public class GuestListUpdatedEvent : BaseEvent
{
    public GuestListUpdatedEvent(GuestList guestList)
    {
        GuestList = guestList;
    }

    public GuestList GuestList { get; }
}
