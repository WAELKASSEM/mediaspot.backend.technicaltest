namespace Mediaspot.Api.Assets.GetAssetById;

public record AssetDto
{
    public Guid Id { get; init;  }
    public string Type { get; protected init; } = string.Empty;
}