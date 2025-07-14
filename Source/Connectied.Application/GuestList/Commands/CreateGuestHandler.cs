using Ardalis.Result;
using Connectied.Application.Repositories;
using Connectied.Domain.GuestList;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.GuestList.Commands;
class CreateGuestHandler : IRequestHandler<CreateGuest, Result<string>>
{
    readonly ILogger<CreateGuestHandler> _logger;
    readonly IRepository<Guest> _repository;

    public CreateGuestHandler(IRepository<Guest> repository, ILogger<CreateGuestHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(CreateGuest request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var guest = new Guest()
            {
                Name = request.Name,
                Group = request.Group,
            };
            var _ = await _repository.AddAsync(guest);
            return Result.Success(guest.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating guest.");
            return Result.Error("An error occurred while creating guest.");
        }
    }
}
