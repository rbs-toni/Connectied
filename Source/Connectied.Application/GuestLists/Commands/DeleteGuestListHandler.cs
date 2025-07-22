using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.Events;
using Connectied.Domain.GuestLists;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Commands;
class DeleteGuestListHandler : IQueryHandler<DeleteGuestList, Result>
{
    readonly ILogger<DeleteGuestListHandler> _logger;
    readonly IRepository<GuestList> _repository;

    public DeleteGuestListHandler(IRepository<GuestList> repository, ILogger<DeleteGuestListHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteGuestList request, CancellationToken cancellationToken)
    {
        try
        {
            var guestList = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (guestList == null)
            {
                return Result.NotFound($"Guest list with ID {request.Id} not found.");
            }
            guestList.AddDomainEvent(new GuestListDeletedEvent(guestList));
            await _repository.DeleteAsync(guestList, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating guest.");
            return Result.Error("An error occurred while creating guest.");
        }
    }
}
