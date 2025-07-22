using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.Guests;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.Dashboard;
class GetGuestStatsHandler : IQueryHandler<GetGuestStats, Result<GuestStats>>
{
    readonly IRepository<Guest> _repository;
    readonly ILogger<GetGuestStatsHandler> _logger;

    public GetGuestStatsHandler(IRepository<Guest> repository, ILogger<GetGuestStatsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<Result<GuestStats>> Handle(GetGuestStats request, CancellationToken cancellationToken)
    {
        try
        {
            var guests = await _repository.ListAsync(new GetGuestStatsSpecs(), cancellationToken);

            var stats = new GuestStats
            {
                Event1Quota = guests.Sum(g => g.Event1Quota),
                Event2Quota = guests.Sum(g => g.Event2Quota),

                Event1Attendance = guests.Sum(g => g.Event1Attendance),
                Event2Attendance = guests.Sum(g => g.Event2Attendance),

                Event1Angpao = guests.Sum(g => g.Event1Angpao),
                Event2Angpao = guests.Sum(g => g.Event2Angpao),

                Event1Gift = guests.Sum(g => g.Event1Gift),
                Event2Gift = guests.Sum(g => g.Event2Gift),

                Event1Souvenir = guests.Sum(g => g.Event1Souvenir),
                Event2Souvenir = guests.Sum(g => g.Event2Souvenir)
            };

            return Result.Success(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating guest.");
            return Result.Error("An error occurred while creating guest.");
        }
    }
}
