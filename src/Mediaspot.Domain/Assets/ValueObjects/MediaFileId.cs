namespace Mediaspot.Domain.Assets.ValueObjects;

public sealed record MediaFileId(Guid Value)
{
    public static MediaFileId New() => new(Guid.CreateVersion7());
}
