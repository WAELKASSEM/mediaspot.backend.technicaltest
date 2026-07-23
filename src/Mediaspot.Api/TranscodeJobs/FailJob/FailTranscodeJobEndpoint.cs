using Mediaspot.Application.Transcoding.Commands.FailJob;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.TranscodeJobs.FailJob;

public static class FailTranscodeJobEndpoint
{
    public static IEndpointRouteBuilder MapFailTranscodeJobEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/{id:guid}/fail", FailTranscodeJobAsync)
            .WithName("Fail Transcode Job")
            .WithDescription("Fail a Transcode Job");

        return endpoints;
    }

    private static async Task<IResult> FailTranscodeJobAsync([FromRoute] Guid id, [FromBody] FailTranscodeJobDto dto, [FromServices] ISender sender)
    {
        var command = dto.ToCommand(id);
        Guid jobId = await sender.Send(command, CancellationToken.None);
        return Results.Ok(jobId);
    }
}
