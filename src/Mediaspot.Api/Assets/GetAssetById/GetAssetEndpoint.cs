using Mediaspot.Application.Assets.Queries.GetById;
using Mediaspot.Domain.Assets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.Assets.GetAssetById;

public static class GetAssetByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetAssetByIdQueryEndpoint(
    this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/{id:guid}", GetAssetByIdAsync)
            .WithName("Get Asset By Id")
            .WithDescription("Get Asset By Id");

        return endpoints;
    }
    private static async Task<IResult> GetAssetByIdAsync([FromRoute] Guid id, [FromServices] ISender sender)
    {
        Asset asset = await sender.Send(new GetAssetByIdQuery(id));
        return Results.Ok(asset);

    }
}
