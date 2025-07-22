using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.GuestLists.Specifications;
using Connectied.Application.Repositories;
using Connectied.Domain.Events;
using Connectied.Domain.GuestLists;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.GuestLists.Commands;
class CreateGuestListHandler : IQueryHandler<CreateGuestList, Result<string>>
{
    readonly IRepository<GuestList> _repository;
    readonly ILogger<GuestList> _logger;

    public CreateGuestListHandler(IRepository<GuestList> repository, ILogger<GuestList> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(CreateGuestList request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Result.Invalid(new ValidationError
                {
                    Identifier = "Name",
                    ErrorMessage = "Name is required."
                });
            }

            var nameSpec = new GuestListByNameSpecs(request.Name);
            var exists = await _repository.AnyAsync(nameSpec, cancellationToken);
            if (exists)
            {
                return Result.Invalid(new ValidationError
                {
                    Identifier = "Name",
                    ErrorMessage = "A guest list with this name already exists."
                });
            }

            var guestList = new GuestList(request.Name);

            var config = request.Configuration;
            if (config != null &&
                (config.Columns?.Count > 0 ||
                 config.Groups?.Count > 0 ||
                 config.IncludedGuests?.Count > 0 ||
                 config.ExcludedGuests?.Count > 0))
            {
                guestList.Configuration = new GuestListConfiguration
                {
                    Columns = config.Columns,
                    Groups = config.Groups,
                    IncludedGuests = config.IncludedGuests,
                    ExcludedGuests = config.ExcludedGuests
                };
            }

            guestList.AddDomainEvent(new GuestListCreatedEvent(guestList));

            await _repository.AddAsync(guestList, cancellationToken);
            return Result.Success(guestList.Id, "Guest list created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating guest list.");
            return Result.Error("An error occurred while creating guest list.");
        }
    }
}
