using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.GuestLists.Specifications;
using Connectied.Application.Repositories;
using Connectied.Domain.Events;
using Connectied.Domain.GuestLists;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Commands;
sealed class UpdateGuestListHandler : ICommandHandler<UpdateGuestList, Result<string>>
{
    readonly IRepository<GuestList> _guestListRepository;
    readonly ILogger<UpdateGuestListHandler> _logger;

    public UpdateGuestListHandler(IRepository<GuestList> guestListRepository, ILogger<UpdateGuestListHandler> logger)
    {
        _guestListRepository = guestListRepository;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(UpdateGuestList request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                return Result.Invalid(new ValidationError(nameof(request.Id), "Id is required"));
            }

            var guestList = await _guestListRepository.FirstOrDefaultAsync(
                new UpdateGuestListSpecs(request.Id), cancellationToken);

            if (guestList is null)
            {
                return Result.NotFound($"GuestList with ID '{request.Id}' not found");
            }

            bool hasChanges = false;

            // Check uniqueness of Name (excluding current guest list)
            if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != guestList.Name)
            {
                var existingWithName = await _guestListRepository.AnyAsync(new GuestListByNameSpecs(request.Name), cancellationToken);

                if (existingWithName)
                {
                    return Result.Conflict($"GuestList name '{request.Name}' is already in use");
                }

                guestList.Name = request.Name;
                hasChanges = true;
            }

            // Update configuration fields only if changed
            if (request.Configuration is not null)
            {
                var config = guestList.Configuration ?? new GuestListConfiguration();

                if (!(request.Configuration.Columns ?? []).SequenceEqual(config.Columns ?? []))
                {
                    config.Columns = request.Configuration.Columns?.ToList() ?? new();
                    hasChanges = true;
                }

                if (!(request.Configuration.Groups ?? []).SequenceEqual(config.Groups ?? []))
                {
                    config.Groups = request.Configuration.Groups?.ToList() ?? new();
                    hasChanges = true;
                }

                if (!(request.Configuration.IncludedGuests ?? []).SequenceEqual(config.IncludedGuests ?? []))
                {
                    config.IncludedGuests = request.Configuration.IncludedGuests?.ToList() ?? new();
                    hasChanges = true;
                }

                if (!(request.Configuration.ExcludedGuests ?? []).SequenceEqual(config.ExcludedGuests ?? []))
                {
                    config.ExcludedGuests = request.Configuration.ExcludedGuests?.ToList() ?? new();
                    hasChanges = true;
                }

                if (guestList.Configuration == null && hasChanges)
                {
                    guestList.Configuration = config;
                }
            }

            if (!hasChanges)
            {
                _logger.LogInformation("No changes detected for GuestList '{GuestListId}'", guestList.Id);
                return Result.Success(guestList.Id); // no update needed
            }

            guestList.AddDomainEvent(new GuestListUpdatedEvent(guestList));
            await _guestListRepository.UpdateAsync(guestList, cancellationToken);
            _logger.LogInformation("GuestList '{GuestListId}' updated successfully", guestList.Id);

            return Result.Success(guestList.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating GuestList with ID '{GuestListId}'", request.Id);
            return Result.Error("An unexpected error occurred while updating the guest list");
        }
    }
}
