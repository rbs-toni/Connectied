using System;
using System.Linq;

namespace Connectied.Application.Persistence;
public interface IGuestHttpClient
{
    Task<IReadOnlyCollection<DummyGuest>> GetLatestGuests(CancellationToken cancellationToken = default);
}
