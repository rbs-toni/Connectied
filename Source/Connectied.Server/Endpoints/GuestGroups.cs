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
            .MapGet(GetGuestGroups);
    }
    async Task<IResult> GetGuestGroups([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetGuestGroups());

        return result.ToMinimalApiResult();
    }
}

