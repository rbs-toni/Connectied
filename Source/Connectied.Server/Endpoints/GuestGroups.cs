using Connectied.Application.Guests;
using Connectied.Application.Guests.Queries;
using Connectied.Server.Extensions;
using Connectied.Server.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Connectied.Server.Endpoints;
public class GuestGroups : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetGuestGroups)
            .MapGet(GetGuestGroup, "/{id}");
    }
    /// <summary>
    /// Retrieves a list of all guest groups.
    /// </summary>
    /// <param name="sender">The mediator used to send the <see cref="GetGuestGroups"/> query.</param>
    /// <returns>An <see cref="IResult"/> containing the list of guest groups.</returns>
    [ProducesResponseType(typeof(IReadOnlyCollection<GuestGroupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> GetGuestGroups([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetGuestGroups());

        return result.ToMinimalApiResult();
    }
    /// <summary>
    /// Retrieves the details of a specific guest group by its ID.
    /// </summary>
    /// <param name="sender">The mediator used to send the <see cref="GetGuestGroup"/> query.</param>
    /// <param name="id">The ID of the guest group to retrieve.</param>
    /// <returns>An <see cref="IResult"/> containing the guest group details if found; otherwise, a not found result.</returns>
    [ProducesResponseType(typeof(GuestGroupDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> GetGuestGroup([FromServices] ISender sender, string id)
    {
        var result = await sender.Send(new GetGuestGroup(id));

        return result.ToMinimalApiResult();
    }
}

