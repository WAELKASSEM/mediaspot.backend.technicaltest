using Mediaspot.Worker.WebApiDtos;
using Microsoft.Extensions.Logging;

namespace Mediaspot.Worker.Transcoders;

public sealed class VideoAssetTranscoder(
    ILogger<VideoAssetTranscoder> logger)
    : IAssetTranscoder
{
    public async Task ExecuteAsync(
        AssetDto asset,
        TranscodeJobDto job,
        CancellationToken ct)
    {
        var videoAsset = (VideoAssetDto)asset;

        logger.LogInformation(
            "Transcoding video asset {Id} ({Resolution}) using preset {Preset}",
            videoAsset.Id,
            videoAsset.Resolution,
            job.Preset);

        await Task.Delay(3000, ct);
    }
}

