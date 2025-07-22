using System;
using System.Linq;

namespace Connectied.Application.Hubs;
public interface IGuestListNotifier
{
    Task NotifyGuestListCreated(string id);
    Task NotifyGuestListUpdated(string id);
    Task NotifyGuestListDeleted(string id);
}
