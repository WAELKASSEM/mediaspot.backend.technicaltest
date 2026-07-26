using Mediaspot.Api.Titles.GetTitleById;
using Mediaspot.Application.Titles.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.Titles.ListTitles;

public static class ListTitlesEndpoint
{
    public static IEndpointRouteBuilder MapListTitlesEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("", ListTitlesAsync)
            .WithName("List Titles")
            .WithDescription("List Titles");

        return endpoints;
    }

    private static async Task<IResult> ListTitlesAsync([FromQuery] Guid? lastSeen, [FromQuery] int? pageSize, [FromServices] ISender sender)
    {
        var query = new ListTitlesQuery(lastSeen, pageSize);
        var result = await sender.Send(query);
        return Results.Ok(result.Select(title => title.ToDto()));

    }
}
