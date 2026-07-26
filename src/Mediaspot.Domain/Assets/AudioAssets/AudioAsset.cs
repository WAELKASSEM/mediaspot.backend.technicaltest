using Mediaspot.Domain.Assets.AudioAssets.Events;
using Mediaspot.Domain.Assets.ValueObjects;

namespace Mediaspot.Domain.Assets.AudioAssets;

public sealed class AudioAsset : Asset
{
    public Duration Duration { get; private set; }
    public int Bitrate { get; private set; }
    public int SampleRate { get; private set; }
    public int Channels { get; private set; }

#pragma warning disable
    private AudioAsset()
    {
    }
#pragma warning enable

    public AudioAsset(
        string externalId,
        Metadata metadata,
        Duration duration,
        int bitrate,
        int sampleRate,
        int channels)
        : base(externalId, metadata)
    {
        Duration = duration;
        Bitrate = bitrate;
        SampleRate = sampleRate;
        Channels = channels;

        Raise(new AudioAssetCreated(Id));
    }

    public void UpdateTechnicalMetadata(
        Duration duration,
        int bitrate,
        int sampleRate,
        int channels)
    {
        Duration = duration;
        Bitrate = bitrate;
        SampleRate = sampleRate;
        Channels = channels;

        Raise(new AudioAssetUpdated(Id));
    }
}

