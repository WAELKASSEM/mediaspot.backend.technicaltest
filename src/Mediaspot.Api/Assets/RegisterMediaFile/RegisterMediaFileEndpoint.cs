using Mediaspot.Application.Assets.Commands.RegisterMediaFile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.Assets.RegisterMediaFile;

public static class RegisterMediaFileEndpoint
{
    public static IEndpointRouteBuilder MapRegisterMediaFileCommandEndpoint(
this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/{id:guid}/files", RegisterMediaFileAsync)
            .WithName("Register Media File")
            .WithDescription("Register Media File");

        return endpoints;
    }
    private static async Task<IResult> RegisterMediaFileAsync([FromRoute] Guid id, [FromBody] RegisterMediaFileDto dto, [FromServices] ISender sender)
    {
        var command = new RegisterMediaFileCommand(id, dto.Path, dto.DurationSeconds);
        Guid mediaFileId = await sender.Send(command);
        return Results.Ok(mediaFileId);
    }

}
