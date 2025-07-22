using Ardalis.Result;
using Ardalis.Specification;
using Connectied.Application.Contracts;
using Connectied.Domain.Guests;
using System.Diagnostics.CodeAnalysis;

namespace Connectied.Application.Guests.Queries;
public record GetGuest : IQuery<Result<GuestDetailsDto>>
{
    public GetGuest(string id)
    {
        Id = id;
    }
    [NotNull]
    public string? Id { get; set; }
}

sealed class GetGuestSpecs : Specification<Guest>
{
    public GetGuestSpecs(string id)
    {
        Query
            .AsNoTracking()
            .AsSplitQuery()
            .Include(g => g.Group)
            .Include(g => g.EventRegistries)
            .Include(g => g.Parent)
            .Include(g => g.Members)
            .Where(g => g.Id == id);
    }
}
