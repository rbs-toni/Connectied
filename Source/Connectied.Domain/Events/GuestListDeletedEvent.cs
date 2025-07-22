using Connectied.Domain.GuestLists;
using System;
using System.Linq;

namespace Connectied.Domain.Events;
public class GuestListDeletedEvent : BaseEvent
{
    public GuestListDeletedEvent(GuestList guestList)
    {
        GuestList = guestList;
    }

    public GuestList GuestList { get; }
}
