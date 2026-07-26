using Mediaspot.Domain.Assets.AudioAssets;

namespace Mediaspot.Api.Assets.GetAssetById;

public sealed record AudioAssetDto : AssetDto
{
    public AudioAssetDto()
    {
        Type = "Audio";
    }
    public int Bitrate { get; init; }
    public int SampleRate { get; init; }
    public int Channels { get; init; }
    //todo: Add more properties later.
}
public static class AudioAssetDtoExtensions
{
    public static AudioAssetDto ToDto(this AudioAsset asset)
    {
        return new()
        {
            Id = asset.Id,
            Bitrate = asset.Bitrate,
            Channels = asset.Channels,
            SampleRate = asset.SampleRate
        };
    }
}