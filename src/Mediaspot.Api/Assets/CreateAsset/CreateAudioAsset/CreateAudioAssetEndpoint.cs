using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.Assets.CreateAsset.CreateAudioAsset;
public static class CreateAudioAssetEndpoint
{
    public static IEndpointRouteBuilder MapCreateAudioAssetCommandEndpoint(
    this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/audios", CreateAudioAssetAsync)
            .WithName("Create Asset")
            .WithDescription("Create Asset");

        return endpoints;
    }
    private static async Task<IResult> CreateAudioAssetAsync([FromBody] CreateAudioAssetDto dto, [FromServices] ISender sender)
    {
        var cmd = dto.ToCommand();
        var id = await sender.Send(cmd);
        return Results.Created("/assets/audios", id);
    }
}
