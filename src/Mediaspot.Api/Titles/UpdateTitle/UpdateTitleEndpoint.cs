using Mediaspot.Application.Titles.Commands.Update;
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

    private static async Task<IResult> UpdateTitleAsync([FromRoute] Guid id, [FromBody] UpdateTitleDto dto, [FromServices] ISender sender)
    {
        UpdateTitleCommand command = dto.ToCommand(id);
        Guid updateId = await sender.Send(command);
        return Results.Ok<Guid>(updateId);
    }
}
