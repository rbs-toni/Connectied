using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.GuestLists;
using Microsoft.Extensions.Logging;

namespace Connectied.Application.GuestLists.Queries;
class GetGuestListsHandler : IQueryHandler<GetGuestLists, Result<IReadOnlyCollection<GuestListDto>>>
{
    private readonly IReadRepository<GuestList> _guestListRepository;
    private readonly ILogger<GetGuestListsHandler> _logger;

    public GetGuestListsHandler(IReadRepository<GuestList> guestListRepository, ILogger<GetGuestListsHandler> logger)
    {
        _guestListRepository = guestListRepository;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<GuestListDto>>> Handle(GetGuestLists request, CancellationToken cancellationToken)
    {
        try
        {
            var guestLists = await _guestListRepository.ListAsync(cancellationToken);
            var guestListDtos = guestLists.Select(gl => new GuestListDto
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
            return Result.Success<IReadOnlyCollection<GuestListDto>>(guestListDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving guest lists.");
            return Result.Error("An error occurred while retrieving guest lists.");
        }
    }
}
