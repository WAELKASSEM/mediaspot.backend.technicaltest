using Mediaspot.Application.Transcoding.Queries.GetJobById;
using Mediaspot.Domain.Transcoding;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.TranscodeJobs.GetJobById;

public static class GetTranscodeJobByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetTranscodeJobByIdEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/{id:guid}", GetTranscodeJobAsync)
            .WithName("Get Transcode Job By Id")
            .WithDescription("Get Transcode Job By Id");

        return endpoints;
    }

    private static async Task<IResult> GetTranscodeJobAsync([FromRoute] Guid id, [FromServices] ISender sender)
    {
        TranscodeJob job = await sender.Send(new GetTranscodeJobByIdQuery(id));
        var dto = job.ToDto();
        return Results.Ok(dto);
    }
}
