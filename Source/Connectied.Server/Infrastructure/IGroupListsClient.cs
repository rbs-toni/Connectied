using Connectied.Application.GuestList;
using System;
using System.Linq;

namespace Connectied.Server.Infrastructure;
public interface IGroupListsClient
{
    Task<IReadOnlyCollection<GuestDto>> GetLatestGroupLists(CancellationToken cancellationToken = default);
}
