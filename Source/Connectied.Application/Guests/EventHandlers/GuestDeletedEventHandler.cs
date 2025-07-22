using Connectied.Application.Hubs;
using Connectied.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.Guests.EventHandlers;
public class GuestDeletedEventHandler : INotificationHandler<GuestDeletedEvent>
{
    readonly ILogger<GuestDeletedEventHandler> _logger;
    readonly IGuestNotifier _notifier;

    public GuestDeletedEventHandler(IGuestNotifier notifier, ILogger<GuestDeletedEventHandler> logger)
    {
        _notifier = notifier;
        _logger = logger;
    }

    public async Task Handle(GuestDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Connectied Domain Event: {DomainEvent}", notification.GetType().Name);

        await _notifier.NotifyGuestDeleted(notification.Guest.Id);
    }
}
