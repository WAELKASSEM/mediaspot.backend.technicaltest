using Mediaspot.Api.Titles.DTOs;
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

    private static Task<IResult> GetTitleAsync([FromRoute] Guid id, [FromServices] ISender store)
    {
        throw new NotImplementedException();
    }
}
