using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.Guests;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Queries;
sealed class GetGuestGroupHandler : IQueryHandler<GetGuestGroup, Result<GuestGroupDetailsDto>>
{
    private readonly IReadRepository<GuestGroup> _guestListRepository;
    private readonly ILogger<GetGuestGroupHandler> _logger;

    public GetGuestGroupHandler(IReadRepository<GuestGroup> guestListRepository, ILogger<GetGuestGroupHandler> logger)
    {
        _guestListRepository = guestListRepository;
        _logger = logger;
    }
    public async Task<Result<GuestGroupDetailsDto>> Handle(GetGuestGroup request, CancellationToken cancellationToken)
    {
        try
        {
            var group = await _guestListRepository
                           .FirstOrDefaultAsync(new GetGuestGroupSpecs(request.Id), cancellationToken);

            if (group is null)
            {
                return Result.NotFound($"Guest group with ID '{request.Id}' was not found.");
            }

            var dto = group.Adapt<GuestGroupDetailsDto>();

            return Result.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching guests for guest group with ID {GroupId}", request.Id);
            return Result.Error("An unexpected error occurred while retrieving guest group members.");
        }
    }
}
