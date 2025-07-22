using Connectied.Application.Hubs;
using Connectied.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.EventHandlers;
public class GuestListUpdatedEventHandler : INotificationHandler<GuestListUpdatedEvent>
{
    readonly ILogger<GuestListUpdatedEventHandler> _logger;
    readonly IGuestListNotifier _notifier;

    public GuestListUpdatedEventHandler(IGuestListNotifier notifier, ILogger<GuestListUpdatedEventHandler> logger)
    {
        _notifier = notifier;
        _logger = logger;
    }

    public async Task Handle(GuestListUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Connectied Domain Event: {DomainEvent}", notification.GetType().Name);

        await _notifier.NotifyGuestListUpdated(notification.GuestList.Id);
    }
}
