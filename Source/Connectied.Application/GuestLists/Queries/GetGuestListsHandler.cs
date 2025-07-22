using Ardalis.Result;
using Ardalis.Specification;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.GuestLists;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Connectied.Application.GuestLists.Queries;
class GetGuestListsHandler : IQueryHandler<GetGuestLists, Result<IReadOnlyCollection<GuestListDto>>>
{
    readonly ILogger<GetGuestListsHandler> _logger;
    readonly IReadRepository<GuestList> _repository;

    public GetGuestListsHandler(IReadRepository<GuestList> repository, ILogger<GetGuestListsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<GuestListDto>>> Handle(
        GetGuestLists request,
        CancellationToken cancellationToken)
    {
        try
        {
            var guestLists = await _repository.ListAsync(new GetGuestListsSpecs(), cancellationToken);

            return Result.Success(guestLists.Adapt<IReadOnlyCollection<GuestListDto>>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching guest lists.");
            return Result.Error("An error occurred while fetching guest lists.");
        }
    }
}

sealed class GetGuestListsSpecs : Specification<GuestList>
{
    public GetGuestListsSpecs()
    {
        Query.AsNoTracking()
            .Include(x => x.Configuration);
    }
}
