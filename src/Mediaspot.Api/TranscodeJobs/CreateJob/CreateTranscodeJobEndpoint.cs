using Mediaspot.Application.Transcoding.Commands.CreateJob;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.TranscodeJobs.CreateJob;

public static class CreateTranscodeJobEndpoint
{
    public static IEndpointRouteBuilder MapCreateTranscodeJobEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("", CreateTranscodeJobAsync)
            .WithName("Create Transcode Job")
            .WithDescription("Create a Transcode Job");

        return endpoints;
    }

    private static async Task<IResult> CreateTranscodeJobAsync([FromBody] CreateTranscodeJobDto dto, [FromServices] ISender sender)
    {
        CreateTranscodeJobCommand command = dto.ToCommand();
        Guid id = await sender.Send(command, CancellationToken.None);
        return Results.Created("/transcode-jobs", id);
    }
}
