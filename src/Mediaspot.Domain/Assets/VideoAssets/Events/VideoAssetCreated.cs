using Mediaspot.Domain.Common;

namespace Mediaspot.Domain.Assets.VideoAssets.Events;

public sealed record VideoAssetCreated(Guid AssetId): IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}