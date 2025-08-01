﻿using Ardalis.Result;
using Ardalis.Specification;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.Events;
using Connectied.Domain.Guests;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Connectied.Application.Guests.Commands;
sealed class CheckInEvent1Handler : ICommandHandler<CheckInEvent1, Result<string>>
{
    readonly IRepository<Guest> _guestRepository;
    readonly ILogger<CheckInEvent1Handler> _logger;

    public CheckInEvent1Handler(
        IRepository<Guest> guestRepository,
        ILogger<CheckInEvent1Handler> logger)
    {
        _guestRepository = guestRepository;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(CheckInEvent1 request, CancellationToken cancellationToken)
    {
        try
        {
            var guest = await _guestRepository.FirstOrDefaultAsync(new CheckInEvent1Specs(request.Id), cancellationToken);
            if (guest is null)
            {
                return Result.NotFound($"Guest with ID '{request.Id}' not found");
            }
            List<GuestRegistry> guestRegistries = [];
            if (request.Registries?.Count > 0)
            {
                foreach (var item in request.Registries)
                {
                    guestRegistries.Add(new GuestRegistry()
                    {
                        EventName = "Event1",
                        Type = item.Type,
                        Quantity = item.Quantity,
                    });
                }
            }

            guest.CheckInEvent1(guestRegistries);
            guest.AddDomainEvent(new GuestUpdatedEvent(guest));

            await _guestRepository.UpdateAsync(guest, cancellationToken);
            _logger.LogInformation("Checked in {Count} guests for Event 1", request.Registries!.Count);
            return Result.Success(request.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during check-in for Event 1");
            return Result.Error("Unexpected error occurred during check-in");
        }
    }
}

sealed class CheckInEvent1Specs : Specification<Guest>
{
    public CheckInEvent1Specs(string id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.EventRegistries);
    }
}

sealed class CheckInEvent1Validator : AbstractValidator<CheckInEvent1>
{
    public CheckInEvent1Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Request Id is required");
    }
}
