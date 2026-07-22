using Mediaspot.Application.Transcoding;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.AudioAssets;
using Mediaspot.Domain.Transcoding;
using Microsoft.Extensions.Logging;

namespace Mediaspot.Infrastructure.Transcoding;


public sealed class AudioAssetTranscoder(
    ILogger<AudioAssetTranscoder> logger)
    : IAssetTranscoder
{
    public async Task ExecuteAsync(
        Asset asset,
        TranscodeJob job,
        CancellationToken ct)
    {
        var audioAsset = (AudioAsset)asset;

        logger.LogInformation(
            "Transcoding audio asset {AssetId} using preset {Preset}",
            audioAsset.Id,
            job.Preset.Value);

        await Task.Delay(3000, ct);
    }

}