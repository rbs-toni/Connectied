using System;
using System.Linq;

namespace Connectied.Server.Hubs;
public interface IGuestListClient
{
    Task GuestListCreated(string id);
    Task GuestListUpdated(string id);
    Task GuestListDeleted(string id);
}
