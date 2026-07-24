using Mediaspot.Application.Transcoding;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.AudioAssets;
using Mediaspot.Domain.Assets.VideoAssets;

namespace Mediaspot.Infrastructure.Transcoding;

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

