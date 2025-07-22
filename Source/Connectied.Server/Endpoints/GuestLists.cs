using Connectied.Application.GuestLists.Commands;
using Connectied.Application.GuestLists.Queries;
using Connectied.Server.Extensions;
using Connectied.Server.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Connectied.Server.Endpoints;
public class GuestLists : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetGuestLists)
            .MapGet(GetGuestList, "/{id}")
            .MapGet(GetGuestsFromGuestList, "/{code}/guests")
            .MapPost(CreateGuestList)
            .MapPut(UpdateGuestList, "/{id}")
            .MapDelete(DeleteGuestList, "/{id}");
    }
    async Task<IResult> GetGuestLists([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetGuestLists());
        return result.ToMinimalApiResult();
    }
    async Task<IResult> GetGuestList([FromServices] ISender sender, string id)
    {
        var result = await sender.Send(new GetGuestListWithGuests(id));
        return result.ToMinimalApiResult();
    }
    async Task<IResult> GetGuestsFromGuestList([FromServices] ISender sender, string code)
    {
        var result = await sender.Send(new GetGuestListWithGuestsByCode(code));
        return result.ToMinimalApiResult();
    }
    async Task<IResult> CreateGuestList([FromServices] ISender sender, [FromBody] CreateGuestList create)
    {
        var result = await sender.Send(create);
        return result.ToMinimalApiResult();
    }
    async Task<IResult> UpdateGuestList([FromServices] ISender sender, string id, [FromBody] UpdateGuestList update)
    {
        var result = await sender.Send(update);
        return result.ToMinimalApiResult();
    }
    async Task<IResult> DeleteGuestList([FromServices] ISender sender, string id)
    {
        var result = await sender.Send(new DeleteGuestList(id));
        return result.ToMinimalApiResult();
    }
}
