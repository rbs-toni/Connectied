using Connectied.Domain.GuestList;
using Connectied.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Connectied.Server.Infrastructure;
public class GuestListsHostedService : BackgroundService
{
    private readonly ILogger<GuestListsHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public GuestListsHostedService(IServiceProvider serviceProvider, ILogger<GuestListsHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("🔁 Starting guest list sync at {Time}", DateTimeOffset.Now);

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var client = scope.ServiceProvider.GetRequiredService<IGroupListsClient>();
                var context = scope.ServiceProvider.GetRequiredService<ConnectiedDbContext>();

                _logger.LogInformation("📥 Fetching guest lists from external source...");

                var fetchedLists = await client.GetLatestGroupLists(stoppingToken);
                _logger.LogInformation("✅ Fetched {Count} guest list(s).", fetchedLists.Count);

                var existingIds = await context.Guests
                    .Select(g => g.Id)
                    .ToListAsync(stoppingToken);

                _logger.LogInformation("📂 Found {Count} existing guest list(s) in the database.", existingIds.Count);

                var newEntities = fetchedLists
                    .Where(dto => !existingIds.Contains(dto.Id))
                    .Select(dto => new Guest
                    {
                        Id = dto.Id,
                        Name = dto.Name,
                        Group = dto.Group,
                        Event1Quota = dto.Event1Quota,
                        Event2Quota = dto.Event2Quota,
                        Event1Attend = dto.Event1Attend,
                        Event2Attend = dto.Event2Attend,
                        Event1Rsvp = dto.Event1Rsvp,
                        Event2Rsvp = dto.Event2Rsvp,
                        Event2AngpaoCount = dto.Event2AngpaoCount,
                        Event2GiftCount = dto.Event2GiftCount,
                        Event2Souvenir = dto.Event2Souvenir,
                        Notes = dto.Notes,
                    })
                    .ToList();

                var skippedCount = fetchedLists.Count - newEntities.Count;
                _logger.LogInformation("🆕 Found {NewCount} new guest list(s) to insert, {SkippedCount} already exist.",
                    newEntities.Count, skippedCount);

                if (newEntities.Count > 0)
                {
                    await context.Guests.AddRangeAsync(newEntities, stoppingToken);
                    await context.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation("💾 Inserted {Count} new guest list(s): {Ids}",
                        newEntities.Count,
                        string.Join(", ", newEntities.Select(e => e.Id)));
                }
                else
                {
                    _logger.LogInformation("📭 No new guest lists to insert.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error during guest list sync: {Message}", ex.Message);
            }

            _logger.LogInformation("⏳ Waiting 5 minutes before next sync...\n");

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
