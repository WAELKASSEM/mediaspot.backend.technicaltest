using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.Titles.UpdateTitle;

public static class UpdateTitleEndpoint
{
    public static IEndpointRouteBuilder MapUpdateTitleEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/{id:guid}", UpdateTitleAsync)
            .WithName("Update Title")
            .WithDescription("Update a Title");

        return endpoints;
    }

    private static Task<IResult> UpdateTitleAsync([FromRoute] Guid id, [FromBody] UpdateTitleDto dto, [FromServices] ISender store)
    {
        throw new NotImplementedException();
    }
}
