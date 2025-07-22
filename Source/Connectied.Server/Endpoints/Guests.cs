using Connectied.Application.Guests.Commands;
using Connectied.Application.Guests.Queries;
using Connectied.Server.Extensions;
using Connectied.Server.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Connectied.Server.Endpoints;
public class Guests : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetGuests)
            .MapGet(SearchGuests, "/search")
            .MapPost(CreateGuest)
            .MapPut(UpdateGuest, "/{id}")
            .MapDelete(DeleteGuest, "/{id}");
    }
    async Task<IResult> GetGuests([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetGuests());

        return result.ToMinimalApiResult();
    }
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

    async Task<IResult> CreateGuest([FromServices] ISender sender, [FromBody] CreateGuest create)
    {
        var result = await sender.Send(create);

        return result.ToMinimalApiResult();
    }

    async Task<IResult> UpdateGuest([FromServices] ISender sender, string id, [FromBody] UpdateGuest update)
    {
        var result = await sender.Send(update);

        return result.ToMinimalApiResult();
    }

    async Task<IResult> DeleteGuest([FromServices] ISender sender, string id)
    {
        var result = await sender.Send(new DeleteGuest(id));

        return result.ToMinimalApiResult();
    }
}

