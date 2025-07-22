using Connectied.Domain.Guests;
using System;
using System.Linq;

namespace Connectied.Domain.Events;

public class GuestUpdatedEvent : BaseEvent
{
    public GuestUpdatedEvent(Guest guest)
    {
        Guest = guest;
    }

    public Guest Guest { get; }
}
