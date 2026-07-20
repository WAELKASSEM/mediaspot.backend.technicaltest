using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.Titles.CreateTitle;

public static class CreateTitleEndpoint
{
    public static IEndpointRouteBuilder MapCreateTitleEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("", CreateTitleAsync)
            .WithName("Create Title")
            .WithDescription("Create a Title");

        return endpoints;
    }

    private static Task<IResult> CreateTitleAsync([FromBody] CreateTitleDto dto, [FromServices] ISender store)
    {
        throw new NotImplementedException();
    }
}
