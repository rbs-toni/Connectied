using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.GuestList;
using Microsoft.Extensions.Logging;

namespace Connectied.Application.GuestList.Queries;
class GetGuestListHandler : IQueryHandler<GetGuestList, Result<IReadOnlyCollection<GuestDto>>>
{
    private readonly IReadRepository<Guest> _guestListRepository;
    private readonly ILogger<GetGuestListHandler> _logger;

    public GetGuestListHandler(IReadRepository<Guest> guestListRepository, ILogger<GetGuestListHandler> logger)
    {
        _guestListRepository = guestListRepository;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<GuestDto>>> Handle(GetGuestList request, CancellationToken cancellationToken)
    {
        try
        {
            var guestLists = await _guestListRepository.ListAsync(cancellationToken);
            var guestListDtos = guestLists.Select(gl => new GuestDto
            {
                Id = gl.Id,
                Name = gl.Name,
                Group = gl.Group,
                Event1Quota = gl.Event1Quota,
                Event2Quota = gl.Event2Quota,
                Event1Rsvp = gl.Event1Rsvp,
                Event2Rsvp = gl.Event2Rsvp,
                Event1Attend = gl.Event1Attend,
                Event2Attend = gl.Event2Attend,
                Event2AngpaoCount = gl.Event2AngpaoCount,
                Event2GiftCount = gl.Event2GiftCount,
                Event2Souvenir = gl.Event2Souvenir,
                Notes = gl.Notes
            }).ToList();
            return Result.Success<IReadOnlyCollection<GuestDto>>(guestListDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving guest lists.");
            return Result.Error("An error occurred while retrieving guest lists.");
        }
    }
}
