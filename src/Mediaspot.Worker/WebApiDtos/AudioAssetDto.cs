namespace Mediaspot.Worker.WebApiDtos;

public sealed record AudioAssetDto : AssetDto
{
    public AudioAssetDto()
    {
    }
    public int Bitrate { get; init; }
    public int SampleRate { get; init; }
    public int Channels { get; init; }
    //todo: Add more properties later.
}