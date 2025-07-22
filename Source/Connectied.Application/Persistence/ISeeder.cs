using System;
using System.Linq;

namespace Connectied.Application.Persistence;
public interface ISeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
