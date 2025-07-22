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
            .MapGet(SearchGuests, "/search")
            .MapPost(CreateGuest)
            .MapPut(UpdateGuest, "/{id}")
            .MapDelete(DeleteGuest, "/{id}")
            .MapPut(CheckInEvent1, "/{id}/check-in/event-1")
            .MapPut(CheckInEvent2, "/{id}/check-in/event-2")
            .MapPut(UpdateGuestRSVP, "/{id}/rsvp");
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
