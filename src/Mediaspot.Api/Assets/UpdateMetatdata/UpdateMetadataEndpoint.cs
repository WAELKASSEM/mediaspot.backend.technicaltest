using Mediaspot.Application.Assets.Commands.UpdateMetadata;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.Assets.UpdateMetatdata;

public static class UpdateMetadataEndpoint
{
    public static IEndpointRouteBuilder MapUpdateMetadataCommandEndpoint(
this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/{id:guid}/metadata", UpdateMetadataAsync)
            .WithName("Update Metadata")
            .WithDescription("Update Metadata");

        return endpoints;
    }
    private static async Task<IResult> UpdateMetadataAsync([FromRoute] Guid id,[FromBody] UpdateMetadataDto dto, [FromServices] ISender sender)
    {
        await sender.Send(new UpdateMetadataCommand(id, dto.Title, dto.Description, dto.Language));
        return Results.NoContent();

    }

}
