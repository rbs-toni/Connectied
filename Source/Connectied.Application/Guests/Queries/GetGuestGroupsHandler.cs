using Ardalis.Result;
using Ardalis.Specification;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.Guests;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Queries;
class GetGuestGroupsHandler : IQueryHandler<GetGuestGroups, Result<IReadOnlyCollection<GuestGroupDto>>>
{
    readonly ILogger<GetGuestGroupsHandler> _logger;
    readonly IReadRepository<GuestGroup> _repository;

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
sealed class GetGuestGroupSpecs : Specification<GuestGroup>
{
    public GetGuestGroupSpecs(string id)
    {
        Query
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Include(x => x.Guests);
    }
}
sealed class GetGuestGroupValidator : AbstractValidator<GetGuestGroup>
{
    public GetGuestGroupValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
