using Connectied.Application.Common.Paging;
using Connectied.Application.Contracts;
using Connectied.Application.Guests;
using Connectied.Application.Guests.Commands;
using Connectied.Application.Guests.Queries;
using Connectied.Domain.Guests;
using Connectied.Server.Extensions;
using Connectied.Server.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Connectied.Server.Endpoints;
public class Guests : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetGuests)
            .MapGet(GetGuest, "/{id}")
            .MapGet(SearchGuests, "/search")
            .MapPost(CreateGuest)
            .MapPut(UpdateGuest, "/{id}")
            .MapDelete(DeleteGuest, "/{id}")
            .MapPut(CheckInEvent1, "/{id}/check-in/event-1")
            .MapPut(CheckInEvent2, "/{id}/check-in/event-2")
            .MapPut(UpdateGuestRSVP, "/{id}/rsvp");
    }
    /// <summary>
    /// Returns all guests.
    /// </summary>
    [ProducesResponseType(typeof(IReadOnlyCollection<GuestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> GetGuests([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetGuests());

        return result.ToMinimalApiResult();
    }

    /// <summary>
    /// Returns a single guest by their ID.
    /// </summary>
    /// <param name="sender">Mediator for dispatching the query.</param>
    /// <param name="id">The ID of the guest to retrieve.</param>
    /// <returns>A result containing the guest details.</returns>
    [ProducesResponseType(typeof(GuestDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> GetGuest([FromServices] ISender sender, string id)
    {
        var result = await sender.Send(new GetGuest(id));

        return result.ToMinimalApiResult();
    }

    /// <summary>
    /// Searches guests using filters.
    /// </summary>
    [ProducesResponseType(typeof(PagedList<GuestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> SearchGuests([FromServices] ISender sender, [AsParameters] SearchGuestListQuery query)
    {
        var filter = new SearchGuests
        {
            Page = query.Page,
            PageSize = query.PageSize,
            OrderBy = query.OrderBy,
            Keyword = query.Keyword,
        };
        var result = await sender.Send(filter);

        return result.ToMinimalApiResult();
    }
    /// <summary>
    /// Creates a new guest.
    /// </summary>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> CreateGuest([FromServices] ISender sender, [FromBody] CreateGuest create)
    {
        var result = await sender.Send(create);

        return result.ToMinimalApiResult();
    }

    /// <summary>
    /// Updates a guest by ID.
    /// </summary>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> UpdateGuest([FromServices] ISender sender, string id, [FromBody] UpdateGuest update)
    {
        var result = await sender.Send(update);

        return result.ToMinimalApiResult();
    }
    /// <summary>
    /// Deletes a guest by ID.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> DeleteGuest([FromServices] ISender sender, string id)
    {
        var result = await sender.Send(new DeleteGuest(id));

        return result.ToMinimalApiResult();
    }

    /// <summary>
    /// Updates RSVP status for a guest.
    /// </summary>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> UpdateGuestRSVP([FromServices] ISender sender, string id, [FromBody] UpdateGuestRSVPModel model)
    {
        var request = new UpdateGuestRSVP
        {
            Id = id,
            Event1Status = model.Event1Status,
            Event2Status = model.Event2Status
        };
        var result = await sender.Send(request);
        return result.ToMinimalApiResult();
    }

    /// <summary>
    /// Checks in a guest for event 1.
    /// </summary>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> CheckInEvent1([FromServices] ISender sender, string id, [FromBody] List<GuestRegistryDto> registries)
    {
        var request = new CheckInEvent1
        {
            Id = id,
            Registries = registries
        };
        var result = await sender.Send(request);
        return result.ToMinimalApiResult();
    }
    /// <summary>
    /// Checks in a guest for event 2.
    /// </summary>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> CheckInEvent2([FromServices] ISender sender, string id, [FromBody] List<GuestRegistryDto> registries)
    {
        var request = new CheckInEvent2
        {
            Id = id,
            Registries = registries
        };
        var result = await sender.Send(request);
        return result.ToMinimalApiResult();
    }

    record UpdateGuestRSVPModel
    {
        public GuestRSVPStatus Event1Status { get; set; }
        public GuestRSVPStatus Event2Status { get; set; }
    }

    record UpdateGuestModel
    {
        public string? Name { get; set; }
        public string? Group { get; set; }
    }
}
