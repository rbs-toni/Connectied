using Connectied.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Connectied.Infrastructure.Persistence;
class DatabaseInitializer : IDatabaseInitializer
{
    readonly ApplicationDbContext _dbContext;
    readonly ILogger<DatabaseInitializer> _logger;
    readonly IEnumerable<ISeeder> _seeders;

    public DatabaseInitializer(
        IServiceProvider serviceProvider,
        ApplicationDbContext dbContext,
        ILogger<DatabaseInitializer> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _seeders = [.. serviceProvider.GetServices<ISeeder>()];
    }

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var pendingMigrations = _dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                await _dbContext.Database.MigrateAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
        }
    }
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Seeding database...");
            await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            // Iterate over each seeder and run them sequentially
            foreach (var seeder in _seeders)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Database seeding was cancelled.");
                    return;
                }
                _logger.LogInformation("Seeding using {seeder}...", seeder.GetType().Name);
                await seeder.SeedAsync(cancellationToken);
            }
            await _dbContext.Database.CommitTransactionAsync(cancellationToken);
            _logger.LogInformation("Database seeding completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
        }
    }
}
