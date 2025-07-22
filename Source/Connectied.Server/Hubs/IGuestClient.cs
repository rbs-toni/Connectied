using System;
using System.Linq;

namespace Connectied.Server.Hubs;
public interface IGuestClient
{
    Task GuestCreated(string id);
    Task GuestUpdated(string id);
    Task GuestDeleted(string id);
}
