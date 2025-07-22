using Connectied.Application.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;

namespace Connectied.Server.Hubs;
public class SignalRGuestListNotifier : IGuestListNotifier
{
    private readonly IHubContext<GuestListHub, IGuestListClient> _hub;

    public SignalRGuestListNotifier(IHubContext<GuestListHub, IGuestListClient> hub)
    {
        _hub = hub;
    }

    public Task NotifyGuestListCreated(string id)
        => _hub.Clients.All.GuestListCreated(id);

    public Task NotifyGuestListUpdated(string id)
        => _hub.Clients.All.GuestListUpdated(id);

    public Task NotifyGuestListDeleted(string id)
        => _hub.Clients.All.GuestListDeleted(id);
}

