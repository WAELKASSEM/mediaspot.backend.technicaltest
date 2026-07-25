using Mediaspot.Worker.WebApiDtos;

namespace Mediaspot.Worker.Transcoders;

public sealed class AssetTranscoderFactory(
    AudioAssetTranscoder audioTranscoder,
    VideoAssetTranscoder videoTranscoder)
{
    public IAssetTranscoder Resolve(AssetDto asset)
    {

        return asset switch
        {
            AudioAssetDto => audioTranscoder,
            VideoAssetDto => videoTranscoder,
            _ => throw new NotSupportedException(
                $"Unsupported asset type '{asset.GetType().Name}'.")
        };
    }
}

