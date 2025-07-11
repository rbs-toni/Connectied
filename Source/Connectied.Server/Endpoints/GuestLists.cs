using Connectied.Application.GuestLists.Queries;
using Connectied.Server.Extensions;
using Connectied.Server.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Connectied.Server.Endpoints;
public class GuestLists : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetGuestLists);
    }
    async Task<IResult> GetGuestLists([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetGuestLists());

        return result.ToMinimalApiResult();
    }
}
