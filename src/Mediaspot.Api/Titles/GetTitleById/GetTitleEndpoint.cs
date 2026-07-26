using Mediaspot.Application.Titles.Queries.GetById;
using Mediaspot.Domain.Titles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.Titles.GetTitleById;

public static class GetTitleEndpoint
{
    public static IEndpointRouteBuilder MapGetTitleByIdEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/{id:guid}", GetTitleAsync)
            .WithName("Get Title By Id")
            .WithDescription("Get Title By Id");

        return endpoints;
    }

    private static async Task<IResult> GetTitleAsync([FromRoute] Guid id, [FromServices] ISender store)
    {
        Title title = await store.Send(new GetTitleByIdQuery(id));
        var dto = title.ToDto();
        return Results.Ok(dto);
    }
}
