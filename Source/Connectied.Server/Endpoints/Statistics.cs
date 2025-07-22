using Connectied.Application.Dashboard;
using Connectied.Server.Extensions;
using Connectied.Server.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Connectied.Server.Endpoints;
public class Statistics : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetGuestStats);
    }
    /// <summary>
    /// Returns guest statistics, such as total invited, attended, not attending, gifts received, etc.
    /// </summary>
    /// <param name="sender">Mediator for dispatching the query.</param>
    /// <returns>A result containing guest statistics.</returns>
    [ProducesResponseType(typeof(GuestStats), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> GetGuestStats([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetGuestStats());

        return result.ToMinimalApiResult();
    }
}
