using Connectied.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Connectied.Server.Hubs;
public class SignalRGuestNotifier : IGuestNotifier
{
    readonly IHubContext<GuestHub, IGuestClient> _hub;

    public SignalRGuestNotifier(IHubContext<GuestHub, IGuestClient> hub)
    {
        _hub = hub;
    }

    public Task NotifyGuestCreated(string id)
        => _hub.Clients.All.GuestCreated(id);
    public Task NotifyGuestDeleted(string id)
        => _hub.Clients.All.GuestDeleted(id);
    public Task NotifyGuestUpdated(string id)
        => _hub.Clients.All.GuestUpdated(id);
}

