using Mediaspot.Api.Assets;
using Mediaspot.Api.Titles;
using Mediaspot.Api.TranscodeJobs;

namespace Mediaspot.Api;

public static class EndpointsMapper
{
    public static IEndpointRouteBuilder MapResourcesEndpoints(
            this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAssetsEndpoints();
        endpoints.MapTitlesEndpoints();
        endpoints.MapTranscodeJobsEndpoints();
        return endpoints;
    }
}
