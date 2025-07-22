using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.Events;
using Connectied.Domain.Guests;
using Microsoft.Extensions.Logging;

namespace Connectied.Application.Guests.Commands;

sealed class UpdateGuestRSVPHandler : ICommandHandler<UpdateGuestRSVP, Result<string>>
{
    readonly IRepository<Guest> _guestRepository;
    readonly ILogger<UpdateGuestRSVPHandler> _logger;

    public UpdateGuestRSVPHandler(IRepository<Guest> guestRepository, ILogger<UpdateGuestRSVPHandler> logger)
    {
        _guestRepository = guestRepository;
        _logger = logger;
    }
    public async Task<Result<string>> Handle(UpdateGuestRSVP request, CancellationToken cancellationToken)
    {
        try
        {
            var guest = await _guestRepository.GetByIdAsync(request.Id, cancellationToken);
            if (guest is null)
            {
                _logger.LogWarning("Guest with ID {GuestId} not found", request.Id);
                return Result.NotFound($"Guest with ID {request.Id} not found");
            }

            guest.Event1RSVPStatus = request.Event1Status;
            guest.Event2RSVPStatus = request.Event2Status;

            guest.AddDomainEvent(new GuestUpdatedEvent(guest));
            await _guestRepository.UpdateAsync(guest, cancellationToken);

            _logger.LogInformation("Updated RSVP status for Guest {GuestId}", guest.Id);
            return Result.Success(guest.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating RSVP for guest {GuestId}", request.Id);
            return Result.Error("Unexpected error occurred while updating RSVP");
        }
    }
}
