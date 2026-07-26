namespace Mediaspot.Worker.WebApiDtos;

public sealed record VideoAssetDto : AssetDto
{
    public VideoAssetDto()
    {
    }
    public string Resolution { get; init; } = string.Empty;
    public decimal FrameRate { get; init; }
    public string Codec { get; init; } = string.Empty;
    //todo: Add more properties later
}
