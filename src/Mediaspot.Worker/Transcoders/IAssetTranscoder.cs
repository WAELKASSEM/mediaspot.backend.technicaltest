using Mediaspot.Worker.WebApiDtos;

namespace Mediaspot.Worker.Transcoders;


public interface IAssetTranscoder
{
    Task ExecuteAsync(
        AssetDto asset,
        TranscodeJobDto job,
        CancellationToken ct);
}

