using Mediaspot.Application.Titles.Commands.Create;
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

    private static async Task<IResult> CreateTitleAsync([FromBody] CreateTitleDto dto, [FromServices] ISender sender)
    {
        CreateTitleCommand command = dto.ToCommand();
        Guid id = await sender.Send(command, CancellationToken.None);
        return Results.Created("/titles",id);

    }
}
