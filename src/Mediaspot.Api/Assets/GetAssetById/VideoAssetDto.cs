using Mediaspot.Domain.Assets.VideoAssets;

namespace Mediaspot.Api.Assets.GetAssetById;

public sealed record VideoAssetDto : AssetDto
{
    public VideoAssetDto()
    {
        Type = "Video";
    }
    public string Resolution { get; init; } = string.Empty;
    public decimal FrameRate { get; init; }
    public string Codec { get; init; } = string.Empty;
    //todo: Add more properties later
}
public static class VideoAssetDtoExtensions
{
    public static VideoAssetDto ToDto(this VideoAsset asset)
    {
        return new()
        {
            Codec = asset.Codec,
            FrameRate = asset.FrameRate,
            Id = asset.Id,
            Resolution = asset.Resolution.ToString(),
        };
    }
}
