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
    async Task<IResult> GetGuestStats([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetGuestStats());

        return result.ToMinimalApiResult();
    }
}
