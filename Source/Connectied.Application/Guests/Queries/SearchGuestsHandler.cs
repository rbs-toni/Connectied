using Ardalis.Result;
using Connectied.Application.Common.Paging;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.Guests;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Queries;
class SearchGuestsHandler : IQueryHandler<SearchGuests, Result<PagedList<GuestDto>>>
{
    readonly ILogger<SearchGuestsHandler> _logger;
    readonly IReadRepository<Guest> _repository;

    public SearchGuestsHandler(IReadRepository<Guest> repository, ILogger<SearchGuestsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<PagedList<GuestDto>>> Handle(SearchGuests request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request);

            var spec = new SearchGuestsSpecs(request);

            var items = await _repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
            var totalCount = await _repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

            var pageList = new PagedList<GuestDto>(items, request.Page, request.PageSize, totalCount);

            return Result.Success(pageList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving guest lists.");
            return Result.Error("An error occurred while retrieving guest lists.");
            throw;
        }
    }
}
