using Mediaspot.Api.Assets.ArchiveAsset;
using Mediaspot.Api.Assets.CreateAsset;
using Mediaspot.Api.Assets.GetAssetById;
using Mediaspot.Api.Assets.RegisterMediaFile;
using Mediaspot.Api.Assets.UpdateMetatdata;

namespace Mediaspot.Api.Assets;

public static class EndpointsGroup
{
    public static IEndpointRouteBuilder MapAssetsEndpoints(
            this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/assets")
            .WithTags("Assets");

        group.MapGetAssetByIdQueryEndpoint();
        group.MapCreateAssetCommandEndpoint();
        group.MapRegisterMediaFileCommandEndpoint();
        group.MapUpdateMetadataCommandEndpoint();
        group.MapArchiveAssetCommandEndpoint();
        return endpoints;
    }
}
