using Mediaspot.Application.Assets.Commands.Archive;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.Assets.ArchiveAsset;

public static class ArchiveAssetEndpoint
{
    public static IEndpointRouteBuilder MapArchiveAssetCommandEndpoint(
    this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/{id:guid}/archive", ArchiveAssetAsync)
            .WithName("Archive Asset")
            .WithDescription("Archive Asset");

        return endpoints;
    }
    private static async Task<IResult> ArchiveAssetAsync([FromRoute] Guid id, [FromServices] ISender sender)
    {
        await sender.Send(new ArchiveAssetCommand(id));
        return Results.NoContent();
    }
}
