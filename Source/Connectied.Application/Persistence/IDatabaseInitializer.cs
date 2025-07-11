using System;
using System.Linq;

namespace Connectied.Application.Persistence;
public interface IDatabaseInitializer
{
    Task MigrateAsync(CancellationToken cancellationToken = default);
    Task SeedAsync(CancellationToken cancellationToken = default);
}

public interface ISeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
