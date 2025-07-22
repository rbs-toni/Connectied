using Connectied.Application.Hubs;
using Connectied.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.EventHandlers;
public class GuestListDeletedEventHandler : INotificationHandler<GuestListDeletedEvent>
{
    readonly ILogger<GuestListDeletedEventHandler> _logger;
    readonly IGuestListNotifier _notifier;

    public GuestListDeletedEventHandler(IGuestListNotifier notifier, ILogger<GuestListDeletedEventHandler> logger)
    {
        _notifier = notifier;
        _logger = logger;
    }

    public async Task Handle(GuestListDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Connectied Domain Event: {DomainEvent}", notification.GetType().Name);

        await _notifier.NotifyGuestListDeleted(notification.GuestList.Id);
    }
}
