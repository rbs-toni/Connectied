using Connectied.Application.Hubs;
using Connectied.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.Guests.EventHandlers;
public class GuestUpdatedEventHandler : INotificationHandler<GuestUpdatedEvent>
{
    readonly ILogger<GuestUpdatedEventHandler> _logger;
    readonly IGuestNotifier _notifier;

    public GuestUpdatedEventHandler(IGuestNotifier notifier, ILogger<GuestUpdatedEventHandler> logger)
    {
        _notifier = notifier;
        _logger = logger;
    }

    public async Task Handle(GuestUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Connectied Domain Event: {DomainEvent}", notification.GetType().Name);

        await _notifier.NotifyGuestUpdated(notification.Guest.Id);
    }
}
