using Ardalis.Result;
using Ardalis.Specification;
using Connectied.Application.Contracts;
using Connectied.Application.Guests;
using Connectied.Application.Repositories;
using Connectied.Domain.GuestLists;
using Connectied.Domain.Guests;

namespace Connectied.Application.GuestLists.Queries;

public class GetGuestsFromGuestListByIdHandler : IQueryHandler<GetGuestsFromGuestListById, Result<IReadOnlyCollection<GuestDto>>>
{
    readonly IReadRepository<GuestList> _guestListRepository;
    readonly IReadRepository<Guest> _guestRepository;

    public GetGuestsFromGuestListByIdHandler(
        IReadRepository<GuestList> guestListRepository,
        IReadRepository<Guest> guestRepository)
    {
        _guestListRepository = guestListRepository;
        _guestRepository = guestRepository;
    }

    public async Task<Result<IReadOnlyCollection<GuestDto>>> Handle(GetGuestsFromGuestListById request, CancellationToken cancellationToken)
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

        var result = guests.Select(g => new GuestDto
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

        return Result.Success((IReadOnlyCollection<GuestDto>)result);
    }
}
