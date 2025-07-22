using Connectied.Application.Hubs;
using Connectied.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.Guests.EventHandlers;
public class GuestCreatedEventHandler : INotificationHandler<GuestCreatedEvent>
{
    readonly ILogger<GuestCreatedEventHandler> _logger;
    readonly IGuestNotifier _notifier;

    public GuestCreatedEventHandler(IGuestNotifier notifier, ILogger<GuestCreatedEventHandler> logger)
    {
        _notifier = notifier;
        _logger = logger;
    }

    public async Task Handle(GuestCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Connectied Domain Event: {DomainEvent}", notification.GetType().Name);

        await _notifier.NotifyGuestCreated(notification.Guest.Id);
    }
}
