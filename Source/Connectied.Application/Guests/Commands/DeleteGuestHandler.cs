using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.Guests;
using Microsoft.Extensions.Logging;

namespace Connectied.Application.Guests.Commands;
class DeleteGuestHandler : ICommandHandler<DeleteGuest, Result>
{
    readonly IRepository<Guest> _repository;
    readonly ILogger<DeleteGuestHandler> _logger;

    public DeleteGuestHandler(IRepository<Guest> repository, ILogger<DeleteGuestHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<Result> Handle(DeleteGuest request, CancellationToken cancellationToken)
    {
        try
        {
            var guest = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (guest is null)
            {
                return Result.NotFound();
            }
            await _repository.DeleteAsync(guest, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting guest.");
            return Result.Error("An error occurred while deleting guest.");
        }
    }
}
