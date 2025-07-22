using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.Guests;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Connectied.Application.Guests.Queries;
sealed class GetGuestHandler : IQueryHandler<GetGuest, Result<GuestDetailsDto>>
{
    readonly IReadRepository<Guest> _repository;
    readonly ILogger<GetGuestHandler> _logger;

    public GetGuestHandler(IReadRepository<Guest> repository, ILogger<GetGuestHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<Result<GuestDetailsDto>> Handle(GetGuest request, CancellationToken cancellationToken)
    {
        try
        {
            var guest = await _repository.FirstOrDefaultAsync(new GetGuestSpecs(request.Id), cancellationToken);
            if (guest is null)
            {
                return Result.NotFound($"Guest with ID '{request.Id}' not found");
            }
            return Result.Success(guest.Adapt<GuestDetailsDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving guest details.");
            return Result.Error("An error occurred while retrieving guest details.");
        }
    }
}
