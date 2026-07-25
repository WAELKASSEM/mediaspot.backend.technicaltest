using Mediaspot.Worker.WebApiDtos;
using Microsoft.Extensions.Logging;

namespace Mediaspot.Worker.Transcoders;


public sealed class AudioAssetTranscoder(
    ILogger<AudioAssetTranscoder> logger)
    : IAssetTranscoder
{
    public async Task ExecuteAsync(
        AssetDto asset,
        TranscodeJobDto job,
        CancellationToken ct)
    {
        var audioAsset = (AudioAssetDto)asset;

        logger.LogInformation(
            "Transcoding audio asset {Id} using preset {Preset}",
            audioAsset.Id,
            job.Preset);

        await Task.Delay(3000, ct);
    }

}