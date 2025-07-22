using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.GuestLists;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Queries;
class GetGuestListHandler : IQueryHandler<GetGuestList, Result<GuestListDto>>
{
    readonly IReadRepository<GuestList> _repository;
    readonly ILogger<GetGuestListHandler> _logger;

    public GetGuestListHandler(IReadRepository<GuestList> repository, ILogger<GetGuestListHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<Result<GuestListDto>> Handle(GetGuestList request, CancellationToken cancellationToken)
    {
        try
        {
            var guestList = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (guestList is null)
            {
                return Result.NotFound();
            }

            return Result.Success(guestList.Adapt<GuestListDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating guest.");
            return Result.Error("An error occurred while creating guest.");
        }
    }
}
