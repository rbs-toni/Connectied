using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Guests;
using Connectied.Application.Repositories;
using Connectied.Domain.GuestLists;
using Connectied.Domain.Guests;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Queries;
public class GetGuestListWithGuestsHandler : IQueryHandler<GetGuestListWithGuests, Result<GuestListWithGuestsDto>>
{
    readonly IReadRepository<GuestList> _guestListRepository;
    readonly IReadRepository<Guest> _guestRepository;
    readonly ILogger<GetGuestListWithGuestsHandler> _logger;

    public GetGuestListWithGuestsHandler(
        IReadRepository<GuestList> guestListRepository,
        IReadRepository<Guest> guestRepository,
        ILogger<GetGuestListWithGuestsHandler> logger)
    {
        _guestListRepository = guestListRepository;
        _guestRepository = guestRepository;
        _logger = logger;
    }

    public async Task<Result<GuestListWithGuestsDto>> Handle(GetGuestListWithGuests request, CancellationToken cancellationToken)
    {
        try
        {
            var guestList = await _guestListRepository.FirstOrDefaultAsync(
                new GetGuestListByIdSpecs(request.Id), cancellationToken);

            if (guestList == null)
            {
                return Result.NotFound();
            }

            var config = guestList.Configuration ?? new GuestListConfiguration();

            var guests = await _guestRepository.ListAsync(
                new GetGuestsFromGuestListConfigurationSpecs(config), cancellationToken);

            var guestDtos = guests.Select(g => new GuestDto
            {
                Id = g.Id,
                Name = g.Name,
                Email = g.Email,
                PhoneNumber = g.PhoneNumber,
                Group = g.Group != null
                    ? new GuestGroupDto
                    {
                        Id = g.Group.Id,
                        Name = g.Group.Name
                    }
                    : null,
                Event1Quota = g.Event1Quota,
                Event2Quota = g.Event2Quota,
                Event1RSVP = g.Event1RSVP,
                Event2RSVP = g.Event2RSVP,
                Event1Attendance = g.Event1Attendance,
                Event2Attendance = g.Event2Attendance,
                Event1Angpao = g.Event1Angpao,
                Event2Angpao = g.Event2Angpao,
                Event1Gift = g.Event1Gift,
                Event2Gift = g.Event2Gift,
                Event1Souvenir = g.Event1Souvenir,
                Event2Souvenir = g.Event2Souvenir,
                Notes = g.Notes
            }).ToList();

            var dto = new GuestListWithGuestsDto
            {
                Id = guestList.Id,
                Name = guestList.Name,
                LinkCode = guestList.LinkCode,
                Configuration = new GuestListConfigurationDto
                {
                    Columns = config.Columns?.ToList(),
                    Groups = config.Groups?.ToList(),
                    IncludedGuests = config.IncludedGuests?.ToList(),
                    ExcludedGuests = config.ExcludedGuests?.ToList()
                },
                Guests = guestDtos
            };

            return Result.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve GuestListWithGuests for GuestListId: {GuestListId}", request.Id);
            _logger.LogCritical(ex, "Unexpected critical error in GuestListWithGuestsHandler");

            return Result.Error("An unexpected error occurred.");
        }
    }
}
