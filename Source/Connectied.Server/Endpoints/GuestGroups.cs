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
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    async Task<IResult> GetGuestGroups([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetGuestGroups());

        return result.ToMinimalApiResult();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    async Task<IResult> GetGuestGroup([FromServices] ISender sender, string id)
    {
        var result = await sender.Send(new GetGuestGroup(id));

        return result.ToMinimalApiResult();
    }
}

