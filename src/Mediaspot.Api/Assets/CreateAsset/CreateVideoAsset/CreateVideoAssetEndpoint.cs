using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.Assets.CreateAsset.CreateVideoAsset;

public static class CreateVideoAssetEndpoint
{
    public static IEndpointRouteBuilder MapCreateVideoAssetCommandEndpoint(
    this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/videos", CreateVideoAssetAsync)
            .WithName("Create Video Asset")
            .WithDescription("Create Video Asset");

        return endpoints;
    }
    private static async Task<IResult> CreateVideoAssetAsync([FromBody] CreateVideoAssetDto dto, [FromServices] ISender sender)
    {
        var cmd = dto.ToCommand();
        var id = await sender.Send(cmd);
        return Results.Created("/assets/videos",id);

    }
}
