using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.Guests;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Connectied.Application.Guests.Queries;
class GetGuestsHandler : IQueryHandler<GetGuests, Result<IReadOnlyCollection<GuestDto>>>
{
    readonly IReadRepository<Guest> _repository;
    readonly ILogger<GetGuestsHandler> _logger;

    public GetGuestsHandler(IReadRepository<Guest> repository, ILogger<GetGuestsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<GuestDto>>> Handle(GetGuests request, CancellationToken cancellationToken)
    {
        try
        {
            var guestLists = await _repository.ListAsync(new GetGuestsSpecs(), cancellationToken);
            //var guestListDtos = guestLists.ConvertAll(gl => new GuestDto
            //{
            //    Id = gl.Id,
            //    Name = gl.Name,
            //    Group = gl.Group != null ? new GuestGroupDto() { Id = gl.Group.Id, Name = gl.Group.Name } : null,
            //    Event1Quota = gl.Event1Quota,
            //    Event2Quota = gl.Event2Quota,
            //    Event1RSVP = gl.Event1RSVP,
            //    Event2RSVP = gl.Event2RSVP,
            //    Event1Attendance = gl.Event1Attendance,
            //    Event2Attendance = gl.Event2Attendance,
            //    Event1Angpao = gl.Event1Angpao,
            //    Event2Angpao = gl.Event2Angpao,
            //    Event1Gift = gl.Event1Gift,
            //    Event2Gift = gl.Event2Gift,
            //    Event1Souvenir = gl.Event1Souvenir,
            //    Event2Souvenir = gl.Event2Souvenir,
            //    Notes = gl.Notes
            //});
            return Result.Success(guestLists.Adapt<IReadOnlyCollection<GuestDto>>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving guest lists.");
            return Result.Error("An error occurred while retrieving guest lists.");
        }
    }
}
