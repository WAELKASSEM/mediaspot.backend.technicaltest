using Mediaspot.Application.Transcoding.Commands.StartJob;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.TranscodeJobs.StartJob;

public static class StartTranscodeJobEndpoint
{
    public static IEndpointRouteBuilder MapStartTranscodeJobEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/{id:guid}/start", StartTranscodeJobAsync)
            .WithName("Start Transcode Job")
            .WithDescription("Start a Transcode Job");

        return endpoints;
    }

    private static async Task<IResult> StartTranscodeJobAsync([FromRoute] Guid id, [FromServices] ISender sender)
    {
        StartTranscodeJobCommand command = new(id);
        Guid jobId = await sender.Send(command, CancellationToken.None);
        return Results.Ok(jobId);
    }
}
