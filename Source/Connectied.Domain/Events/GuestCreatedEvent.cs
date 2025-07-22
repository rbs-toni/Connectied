using Connectied.Domain.Guests;
using System;
using System.Linq;

namespace Connectied.Domain.Events;
public class GuestCreatedEvent : BaseEvent
{
    public GuestCreatedEvent(Guest guest)
    {
        Guest = guest;
    }

    public Guest Guest { get; }
}
