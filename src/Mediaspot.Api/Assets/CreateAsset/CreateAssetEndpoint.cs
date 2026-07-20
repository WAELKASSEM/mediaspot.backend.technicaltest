using Mediaspot.Application.Assets.Commands.Create;
using Mediaspot.Application.Assets.Queries.GetById;
using Mediaspot.Domain.Assets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.Assets.CreateAsset;

public static class CreateAssetEndpoint
{
    public static IEndpointRouteBuilder MapCreateAssetCommandEndpoint(
    this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("", CreateAssetAsync)
            .WithName("Create Asset")
            .WithDescription("Create Asset");

        return endpoints;
    }
    private static async Task<IResult> CreateAssetAsync([FromBody] CreateAssetCommand cmd, [FromServices] ISender sender)
    {
        return Results.Created("/assets", new { id = await sender.Send(cmd) });

    }
}
