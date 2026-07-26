using Mediaspot.Domain.Common;

namespace Mediaspot.Domain.Assets.VideoAssets.Events;

public sealed record VideoAssetUpdated(Guid AssetId): IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
