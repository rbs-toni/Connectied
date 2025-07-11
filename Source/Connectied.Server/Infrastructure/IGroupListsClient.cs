using Connectied.Application.GuestLists;
using System;
using System.Linq;

namespace Connectied.Server.Infrastructure;
public interface IGroupListsClient
{
    Task<IReadOnlyCollection<GuestListDto>> GetLatestGroupLists(CancellationToken cancellationToken = default);
}
