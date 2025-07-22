using System;
using System.Linq;

namespace Connectied.Server.Hubs;
public interface IGuestListClient
{
    Task GuestListCreated(string guestListId);
    Task GuestListUpdated(string guestListId);
    Task GuestListDeleted(string guestListId);
}
