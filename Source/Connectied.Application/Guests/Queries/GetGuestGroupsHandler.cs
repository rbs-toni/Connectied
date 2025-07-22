using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.Guests;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Queries;
class GetGuestGroupsHandler : IQueryHandler<GetGuestGroups, Result<IReadOnlyCollection<GuestGroupDto>>>
{
    readonly IReadRepository<GuestGroup> _repository;
    readonly ILogger<GetGuestGroupsHandler> _logger;

    public GetGuestGroupsHandler(IReadRepository<GuestGroup> repository, ILogger<GetGuestGroupsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<GuestGroupDto>>> Handle(GetGuestGroups request, CancellationToken cancellationToken)
    {
        try
        {
            var groups = await _repository.ListAsync(new GetGuestGroupsSpecs(), cancellationToken);

            if (groups is null || !groups.Any())
            {
                _logger.LogInformation("No guest groups found.");
                return Result.NotFound("No guest groups found.");
            }

            var dto = groups.Adapt<IReadOnlyCollection<GuestGroupDto>>();
            _logger.LogInformation("Retrieved {Count} guest groups.", dto.Count);
            return Result.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving guest groups.");
            return Result.Error("An error occurred while retrieving guest groups.");
        }
    }
}
