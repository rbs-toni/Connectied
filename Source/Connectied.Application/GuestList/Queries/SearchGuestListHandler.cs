using Ardalis.Result;
using Connectied.Application.Common.Paging;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.GuestList;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.GuestList.Queries;
class SearchGuestListHandler : IQueryHandler<SearchGuestList, Result<PagedList<GuestDto>>>
{
    readonly ILogger<SearchGuestListHandler> _logger;
    readonly IReadRepository<Guest> _repository;

    public SearchGuestListHandler(IReadRepository<Guest> repository, ILogger<SearchGuestListHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<PagedList<GuestDto>>> Handle(SearchGuestList request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request);

            var spec = new SearchGuestListSpecs(request);

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
