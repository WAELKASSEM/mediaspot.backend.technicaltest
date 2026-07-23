namespace Mediaspot.Api.TranscodeJobs;

using Mediaspot.Api.TranscodeJobs.CreateJob;
using Mediaspot.Api.TranscodeJobs.StartJob;
using Mediaspot.Api.TranscodeJobs.CompleteJob;
using Mediaspot.Api.TranscodeJobs.FailJob;
using Mediaspot.Api.TranscodeJobs.GetJobById;

public static class EndpointsGroup
{
    public static IEndpointRouteBuilder MapTranscodeJobsEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/transcode-jobs").WithTags("Transcode Jobs");

        group.MapCreateTranscodeJobEndpoint();
        group.MapGetTranscodeJobByIdEndpoint();
        group.MapStartTranscodeJobEndpoint();
        group.MapCompleteTranscodeJobEndpoint();
        group.MapFailTranscodeJobEndpoint();

        return endpoints;
    }
}
