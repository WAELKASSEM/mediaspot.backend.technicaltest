using Mediaspot.Application.Transcoding;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.AudioAssets;
using Mediaspot.Domain.Assets.VideoAssets;
using Mediaspot.Domain.Transcoding;
using Microsoft.Extensions.Logging;

namespace Mediaspot.Infrastructure.Transcoding;

public sealed class VideoAssetTranscoder(
    ILogger<VideoAssetTranscoder> logger)
    : IAssetTranscoder
{
    public async Task ExecuteAsync(
        Asset asset,
        TranscodeJob job,
        CancellationToken ct)
    {
        var videoAsset = (VideoAsset)asset;

        logger.LogInformation(
            "Transcoding video asset {AssetId} ({Width}x{Height}) using preset {Preset}",
            videoAsset.Id,
            videoAsset.Resolution.Width,
            videoAsset.Resolution.Height,
            job.Preset.Value);

        await Task.Delay(3000, ct);
    }
}


public sealed class AssetTranscoderFactory(
    AudioAssetTranscoder audioTranscoder,
    VideoAssetTranscoder videoTranscoder)
{
    public IAssetTranscoder Resolve(Asset asset)
    {
        return asset switch
        {
            AudioAsset => audioTranscoder,
            VideoAsset => videoTranscoder,
            _ => throw new NotSupportedException(
                $"Unsupported asset type '{asset.GetType().Name}'.")
        };
    }
}

