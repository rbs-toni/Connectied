using Connectied.Application.GuestList.Commands;
using Connectied.Application.GuestList.Queries;
using Connectied.Server.Extensions;
using Connectied.Server.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection;

namespace Connectied.Server.Endpoints;
public class GuestList : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetGuestList)
            .MapGet(SearchGuestList, "/search")
            .MapPost(CreateGuest)
            .MapPut(UpdateGuest, "/{id}")
            .MapDelete(DeleteGuest, "/{id}");
    }
    async Task<IResult> GetGuestList([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetGuestList());

        return result.ToMinimalApiResult();
    }
    async Task<IResult> SearchGuestList([FromServices] ISender sender, [AsParameters] SearchGuestListQuery query)
    {
        var filter = new SearchGuestList
        {
            Page = query.Page,
            PageSize = query.PageSize,
            OrderBy = query.OrderBy,
            Keyword = query.Keyword,
        };
        var result = await sender.Send(filter);

        return result.ToMinimalApiResult();
    }

    class SearchGuestListQuery
    {
        public required int Page { get; init; } = 1;
        public required int PageSize { get; init; } = 10;
        public string[]? OrderBy { get; init; }
        public string? Keyword { get; init; }
        public string? AdvancedSearch { get; init; }
        public string? AdvancedFilter { get; init; }

        public static ValueTask<SearchGuestListQuery?> BindAsync(HttpContext context, ParameterInfo parameter)
        {
            const string currentPageKey = "page";
            const string pageSizeKey = "pageSize";
            const string orderByKey = "orderBy";
            const string keywordKey = "keyword";

            var query = context.Request.Query;

            _ = int.TryParse(query[currentPageKey], out int page);
            _ = int.TryParse(query[pageSizeKey], out int pageSize);
            var orderBy = query[orderByKey].ToArray();
            var keyword = query[keywordKey].ToString();

            var result = new SearchGuestListQuery()
            {
                Page = page == 0 ? 1 : page,
                PageSize = pageSize == 0 ? 10 : pageSize,
                OrderBy = orderBy.Length > 0 ? orderBy : null,
                Keyword = keyword
            };

            return ValueTask.FromResult<SearchGuestListQuery?>(result);
        }
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
