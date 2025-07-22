using Ardalis.Result;
using Connectied.Application.Repositories;
using Connectied.Domain.Guests;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.Guests.Commands;
class UpdateGuestHandler : IRequestHandler<UpdateGuest, Result<string>>
{
    readonly ILogger<UpdateGuest> _logger;
    readonly IRepository<Guest> _repository;

    public UpdateGuestHandler(IRepository<Guest> repository, ILogger<UpdateGuest> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(UpdateGuest request, CancellationToken cancellationToken)
    {
        try
        {
            var existedGuest = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existedGuest is null)
            {
                return Result.NotFound();
            }
            throw new NotImplementedException("UpdateGuestHandler is not implemented yet.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating guest.");
            return Result.Error("An error occurred while updating guest.");
        }
    }
}
