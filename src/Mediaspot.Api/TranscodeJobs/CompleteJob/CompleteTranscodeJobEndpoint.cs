using Mediaspot.Application.Transcoding.Commands.CompleteJob;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.TranscodeJobs.CompleteJob;

public static class CompleteTranscodeJobEndpoint
{
    public static IEndpointRouteBuilder MapCompleteTranscodeJobEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/{id:guid}/complete", CompleteTranscodeJobAsync)
            .WithName("Complete Transcode Job")
            .WithDescription("Complete a Transcode Job");

        return endpoints;
    }

    private static async Task<IResult> CompleteTranscodeJobAsync([FromRoute] Guid id, [FromServices] ISender sender)
    {
        CompleteTranscodeJobCommand command = new(id);
        Guid jobId = await sender.Send(command, CancellationToken.None);
        return Results.Ok(jobId);
    }
}
