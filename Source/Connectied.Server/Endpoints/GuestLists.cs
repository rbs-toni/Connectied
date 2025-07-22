using Connectied.Application.Contracts;
using Connectied.Application.GuestLists;
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
    /// <summary>
    /// Retrieves all guest lists.
    /// </summary>
    /// <param name="sender">The mediator used to send the query.</param>
    /// <returns>200 OK with a list of guest lists.</returns>
    [ProducesResponseType(typeof(IReadOnlyCollection<GuestListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> GetGuestLists([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetGuestLists());
        return result.ToMinimalApiResult();
    }

    /// <summary>
    /// Retrieves a guest list and its guests by ID.
    /// </summary>
    /// <param name="sender">The mediator used to send the query.</param>
    /// <param name="id">The guest list ID.</param>
    /// <returns>200 OK with guest list details and guests, 404 if not found.</returns>
    [ProducesResponseType(typeof(GuestListWithGuestsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> GetGuestList([FromServices] ISender sender, string id)
    {
        var result = await sender.Send(new GetGuestListWithGuests(id));
        return result.ToMinimalApiResult();
    }

    /// <summary>
    /// Retrieves a guest list and its guests by code.
    /// </summary>
    /// <param name="sender">The mediator used to send the query.</param>
    /// <param name="code">The guest list code.</param>
    /// <returns>200 OK with guest list details and guests, 404 if not found.</returns>
    [ProducesResponseType(typeof(GuestListWithGuestsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> GetGuestsFromGuestList([FromServices] ISender sender, string code)
    {
        var result = await sender.Send(new GetGuestListWithGuestsByCode(code));
        return result.ToMinimalApiResult();
    }

    /// <summary>
    /// Creates a new guest list.
    /// </summary>
    /// <param name="sender">The mediator used to send the query.</param>
    /// <param name="create">The data to create the guest list.</param>
    /// <returns>201 Created with the created guest list or its ID.</returns>
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> CreateGuestList([FromServices] ISender sender, [FromBody] CreateGuestList create)
    {
        var result = await sender.Send(create);
        return result.ToMinimalApiResult();
    }

    /// <summary>
    /// Updates an existing guest list.
    /// </summary>
    /// <param name="sender">The mediator used to send the query.</param>
    /// <param name="id">The guest list ID.</param>
    /// <param name="model">The updated data.</param>
    /// <returns>204 No Content on success, 404 if not found.</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> UpdateGuestList([FromServices] ISender sender, string id, [FromBody] UpdateGuestListModel model)
    {
        var request = new UpdateGuestList()
        {
            Id = id,
            Name = model.Name,
            Configuration = model.Configuration
        };
        var result = await sender.Send(request);
        return result.ToMinimalApiResult();
    }

    /// <summary>
    /// Deletes a guest list by ID.
    /// </summary>
    /// <param name="sender">The mediator used to send the query.</param>
    /// <param name="id">The ID of the guest list to delete.</param>
    /// <returns>204 No Content if deleted, 404 if not found.</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    async Task<IResult> DeleteGuestList([FromServices] ISender sender, string id)
    {
        var result = await sender.Send(new DeleteGuestList(id));
        return result.ToMinimalApiResult();
    }

    record UpdateGuestListModel
    {
        public string? Name { get; set; }
        public GuestListConfigurationDto? Configuration { get; set; }
    }
}
