using Ardalis.Result;
using Connectied.Application.Repositories;
using Connectied.Domain.GuestList;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.GuestList.Commands;
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
            bool changed = false;
            if (!string.IsNullOrWhiteSpace(request.Name) && existedGuest.Name != request.Name)
            {
                changed = true;
                existedGuest.Name = request.Name;
            }
            if (!string.IsNullOrWhiteSpace(request.Group) && existedGuest.Group != request.Group)
            {
                changed = true;
                existedGuest.Group = request.Group;
            }
            if (changed)
            {
                await _repository.UpdateAsync(existedGuest, cancellationToken);
            }

            return Result.Success(existedGuest.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating guest.");
            return Result.Error("An error occurred while updating guest.");
        }
    }
}
