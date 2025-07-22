using Connectied.Application.Hubs;
using Connectied.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.EventHandlers;
public class GuestListCreatedEventHandler : INotificationHandler<GuestListCreatedEvent>
{
    readonly ILogger<GuestListCreatedEventHandler> _logger;
    readonly IGuestListNotifier _notifier;

    public GuestListCreatedEventHandler(IGuestListNotifier notifier, ILogger<GuestListCreatedEventHandler> logger)
    {
        _notifier = notifier;
        _logger = logger;
    }

    public async Task Handle(GuestListCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Connectied Domain Event: {DomainEvent}", notification.GetType().Name);

        await _notifier.NotifyGuestListCreated(notification.GuestList.Id);
    }
}
