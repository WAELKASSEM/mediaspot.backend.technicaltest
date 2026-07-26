using Mediaspot.Domain.Assets.ValueObjects;
using Mediaspot.Domain.Assets.VideoAssets.Events;
using Mediaspot.Domain.Assets.VideoAssets.ValueObjects;

namespace Mediaspot.Domain.Assets.VideoAssets;

public sealed class VideoAsset : Asset
{
    public Duration Duration { get; private set; }
    public Resolution Resolution { get; private set; }
    public decimal FrameRate { get; private set; }
    public string Codec { get; private set; }

#pragma warning disable
    private VideoAsset()
    {
    }
#pragma warning enable

    public VideoAsset(
        string externalId,
        Metadata metadata,
        Duration duration,
        Resolution resolution,
        decimal frameRate,
        string codec)
        : base(externalId, metadata)
    {
        Duration = duration;
        Resolution = resolution;
        FrameRate = frameRate;
        Codec = codec;

        Raise(new VideoAssetCreated(Id));
    }

    public void UpdateTechnicalMetadata(
        Duration duration,
        Resolution resolution,
        decimal frameRate,
        string codec)
    {
        Duration = duration;
        Resolution = resolution;
        FrameRate = frameRate;
        Codec = codec;

        Raise(new VideoAssetUpdated(Id));
    }
}

