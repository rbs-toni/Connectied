using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;

namespace Connectied.Server.Hubs;
public class GuestListHub : Hub<IGuestListClient>
{
}
