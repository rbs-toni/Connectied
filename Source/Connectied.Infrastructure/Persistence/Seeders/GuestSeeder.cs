using Connectied.Application.Persistence;
using Connectied.Domain.Guests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Infrastructure.Persistence.Seeders;
class GuestSeeder : ISeeder
{
    readonly ConnectiedDbContext _context;
    readonly IGuestHttpClient _httpClient;
    readonly ILogger<GuestSeeder> _logger;

    public GuestSeeder(ConnectiedDbContext context, IGuestHttpClient httpClient, ILogger<GuestSeeder> logger)
    {
        _context = context;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("🔁 Starting guest list sync at {Time}", DateTimeOffset.Now);

        try
        {
            _logger.LogInformation("📥 Fetching guest lists from external source...");
            var fetchedGuests = await _httpClient.GetLatestGuests(cancellationToken);
            _logger.LogInformation("✅ Fetched {Count} guest(s).", fetchedGuests.Count);

            var existingIds = await _context.Guests
                .Select(g => g.Id)
                .ToListAsync(cancellationToken);
            _logger.LogInformation("📂 Found {Count} existing guest(s) in the database.", existingIds.Count);

            var allGroups = await _context.GuestGroups.ToListAsync(cancellationToken);
            var newEntities = new List<Guest>();

            foreach (var dto in fetchedGuests)
            {
                if (existingIds.Contains(dto.Id))
                {
                    continue;
                }

                GuestGroup? matchedGroup = null;

                if (!string.IsNullOrWhiteSpace(dto.Group))
                {
                    matchedGroup = allGroups.FirstOrDefault(g => g.Name == dto.Group);

                    if (matchedGroup == null)
                    {
                        matchedGroup = new GuestGroup
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = dto.Group
                        };
                        _context.GuestGroups.Add(matchedGroup);
                        allGroups.Add(matchedGroup);
                    }
                }

                var guest = new Guest
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Event1Quota = dto.Event1Quota,
                    Event2Quota = dto.Event2Quota,
                    Event1RSVP = dto.Event1RSVP,
                    Event2RSVP = dto.Event2RSVP,
                    Event1Attendance = dto.Event1Attendance,
                    Event2Attendance = dto.Event2Attendance,
                    Event1Angpao = dto.Event1Angpao,
                    Event2Angpao = dto.Event2Angpao,
                    Event1Gift = dto.Event1Gift,
                    Event2Gift = dto.Event2Gift,
                    Event1Souvenir = dto.Event1Souvenir,
                    Event2Souvenir = dto.Event2Souvenir,
                    GroupId = matchedGroup?.Id,
                    Group = matchedGroup,
                    Notes = dto.Notes,
                };

                newEntities.Add(guest);
            }

            var skippedCount = fetchedGuests.Count - newEntities.Count;
            _logger.LogInformation("🆕 Found {NewCount} new guest(s) to insert, {SkippedCount} already exist.",
                newEntities.Count, skippedCount);

            if (newEntities.Count > 0)
            {
                await _context.Guests.AddRangeAsync(newEntities, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("💾 Inserted {Count} new guest(s): {Ids}",
                    newEntities.Count,
                    string.Join(", ", newEntities.Select(e => e.Id)));
            }
            else
            {
                _logger.LogInformation("📭 No new guests to insert.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error during guest sync: {Message}", ex.Message);
        }
    }
}
