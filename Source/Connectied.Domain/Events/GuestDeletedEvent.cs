using Connectied.Domain.Guests;
using System;
using System.Linq;

namespace Connectied.Domain.Events;

public class GuestDeletedEvent : BaseEvent
{
    public GuestDeletedEvent(Guest guest)
    {
        Guest = guest;
    }

    public Guest Guest { get; }
}
